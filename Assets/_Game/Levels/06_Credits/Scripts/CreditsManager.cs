using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup), typeof(CanvasGroup))]
public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    float _delay = 2f;

    [SerializeField]
    float _crawlTime = 30f;

    [SerializeField]
    float _timeBeforeEnd = 4f;

    [SerializeField]
    [Tooltip("A CSV file of departments and participants/roles")]
    TextAsset _rosterAsset = null;

    [SerializeField]
    GameObject _departmentPrefab = null;

    RectTransform _rectTrans = null;
    CanvasGroup _canvasGroup = null;
    VerticalLayoutGroup _verticalLayoutGroup = null;

    void Awake()
    {
        _rectTrans = transform as RectTransform;
        _canvasGroup = GetComponent<CanvasGroup>();
        _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    void Start()
    {
        if (_rosterAsset == null)
        {
            Debug.LogWarning("No roster was provided");
            return;
        }
        else if (_departmentPrefab == null)
        {
            Debug.LogWarning("No department prefab was provided.");
            return;
        }

        // generate ui elements
        Department[] departments = ParseForDepartments();
        List<RectTransform> ui = GenerateDepartments(departments, transform);

        // hide canvas because it will take multiple frames to configure and move it
        _canvasGroup.alpha = 0;

        // wait a frame before setting heights to allow text mesh pro to update the renderer
        StartCoroutine(WaitAFrame(
        () =>
        {
            ValidateDepartmentLayout(ui, _rectTrans);
            StartCoroutine(WaitAFrame(
            () =>
            {
                // update height to fit content
                var newSize = _rectTrans.sizeDelta;
                newSize.y = GetComponent<LayoutGroup>().minHeight + 150;
                _rectTrans.sizeDelta = newSize;

                // move below viewport
                _rectTrans.anchorMax = _rectTrans.anchorMin = new Vector2(0.5f, 0);
                _rectTrans.pivot = new Vector2(0.5f, 1);

                // show the credits
                _canvasGroup.alpha = 1;

                StartCoroutine(Mechanics.Dialog.Tweens.WaitBefore(
                    _delay,
                    () => StartCoroutine(AnimateCredits())
                ));
            }));
        }));
    }

    IEnumerator AnimateCredits()
    {
        Vector3 from = transform.localPosition;
        Vector3 to = transform.localPosition + new Vector3(0, _rectTrans.sizeDelta.y + GetComponentInParent<CanvasScaler>().referenceResolution.y, 0);

        var timeElapsed = 0f;
        while (timeElapsed < _crawlTime)
        {
            transform.localPosition = Vector3.Lerp(from, to, timeElapsed / _crawlTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;

        StartCoroutine(Mechanics.Dialog.Tweens.WaitBefore(_timeBeforeEnd,
        () =>
        {
            Debug.Log("Credits are done.");
            StartCoroutine(Mechanics.Dialog.Tweens.WaitBefore(3f,
            () =>
            {
                Debug.LogWarning("Seriously... go home!");
            }));
        }));

        yield break;
    }

    /// <summary>
    /// Parses <see cref="_rosterAsset"/> for departments of people/roles
    /// </summary>
    /// <returns></returns>
    Department[] ParseForDepartments()
    {
        if (_rosterAsset == null) return null;

        string file = _rosterAsset.text;
        string[] src_departments = file.Split(new string[] { ",,," }, System.StringSplitOptions.None);
        List<Department> departments = new List<Department>();

        try
        {
            foreach (string src_department in src_departments)
            {
                string[] lines = src_department.Trim().Split('\n');

                Department curDepartment = new Department();
                foreach (string line in lines)
                {
                    string[] line_split = line.Split(',');


                    // get department name from the first entry of the department
                    if (curDepartment.Name == null)
                    {
                        curDepartment.Name = line_split[0].Trim();
                    }

                    // put together person's info
                    curDepartment.Roles.Add(line_split[1].Trim());
                    curDepartment.People.Add($"{line_split[2]} {line_split[3]}".Trim());
                }

                departments.Add(curDepartment);
            }
        }
        catch
        {
            Debug.LogError("CreditsManager.ParseForDepartments: Unable to parse _rosterAsset for departments.");
            return null;
        }

        return departments.ToArray();
    }

    /// <summary>
    /// Creates instance of <see cref="_departmentPrefab"/> and configures it with <see cref="ConfigureWithDepartment(Department, TMP_Text, TMP_Text, TMP_Text)"/>
    /// </summary>
    /// <param name="departments"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    List<RectTransform> GenerateDepartments(Department[] departments, Transform parent)
    {
        List<RectTransform> blocks = new List<RectTransform>();

        foreach (Department department in departments)
        {
            GameObject go = Instantiate(_departmentPrefab);
            go.transform.SetParent(parent, false);
            blocks.Add(go.transform as RectTransform);
            go.name = $"GRP_{department.Name}";

            TMP_Text txt_department = go.transform.Find("TXT_Department").GetComponent<TMP_Text>();
            TMP_Text txt_roles = go.transform.Find("TXT_Roles").GetComponent<TMP_Text>();
            TMP_Text txt_people = go.transform.Find("TXT_People").GetComponent<TMP_Text>();

            ConfigureWithDepartment(department, txt_department, txt_roles, txt_people);
        }

        return blocks;
    }

    /// <summary>
    /// Uploads data to text objects
    /// </summary>
    /// <param name="department"></param>
    /// <param name="txt_department"></param>
    /// <param name="txt_roles"></param>
    /// <param name="txt_people"></param>
    void ConfigureWithDepartment(Department department, TMP_Text txt_department, TMP_Text txt_roles, TMP_Text txt_people)
    {
        txt_department.text = department.Name;
        txt_people.text = string.Join("\n", department.People);

        string str_roles = "";
        string curRole = "";
        foreach (string role in department.Roles)
        {
            if (role != curRole)
            {
                curRole = role;
                str_roles += $"{role}\n";
            }
            else
            {
                str_roles += "\n";
            }
        }

        txt_roles.text = str_roles;
    }

    /// <summary>
    /// Updates child object's heights to allow <see cref="LayoutGroup"/> to work properly.
    /// </summary>
    /// <param name="departments"></param>
    /// <param name="parent"></param>
    void ValidateDepartmentLayout(List<RectTransform> departments, RectTransform parent)
    {
        foreach (var trans in departments)
        {
            TMP_Text txt_department = trans.Find("TXT_Department").GetComponent<TMP_Text>();
            TMP_Text txt_roles      = trans.Find("TXT_Roles").GetComponent<TMP_Text>();
            TMP_Text txt_people     = trans.Find("TXT_People").GetComponent<TMP_Text>();

            Vector2 size = trans.sizeDelta;
            //size.y = 50 + txt_people.transform.localPosition.y - Mathf.Max(txt_people.renderedHeight, txt_roles.renderedHeight);
            size.y = txt_department.renderedHeight + Mathf.Max(txt_people.renderedHeight, txt_roles.renderedHeight) + 50;
            trans.sizeDelta = size;
        }

        // force layout to recalculate children's positions
        _verticalLayoutGroup.enabled = false;
        _verticalLayoutGroup.enabled = true;
    }

    IEnumerator WaitAFrame(System.Action callback)
    {
        yield return null;
        callback?.Invoke();
    }

    string PrettyPrint(Department[] departments)
    {
        string output = "";

        foreach (var department in departments)
        {
            if (department == null) continue;

            output += $"{department.Name}\n";
            for (int i = 0; i < department.Roles.Count; i++)
            {
                output += $"{department.Roles[i]} | {department.People[i]}\n";
            }
            output += "\n";
        }

        return output;
    }

    class Department
    {
        public string Name = null;
        public List<string> Roles = new List<string>();
        public List<string> People = new List<string>();

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
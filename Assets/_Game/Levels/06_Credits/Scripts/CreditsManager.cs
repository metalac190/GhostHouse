using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A CSV file of departments and participants/roles")]
    TextAsset _rosterAsset = null;

    void Start()
    {
        Department[] departments = ParseForDepartments();
        print(PrettyPrint(departments));
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

    void ConfigureWithDepartment(Department department, TMP_Text txt_department, TMP_Text txt_roles, TMP_Text txt_people)
    {
        txt_department.text = department.Name;
        txt_roles.text = string.Join("\n", department.People);

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

        txt_people.text = str_roles;
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
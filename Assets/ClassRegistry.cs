using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassRegistry", menuName = "ClassRegistry")]
public class ClassRegistry: ScriptableObject
{
    [SerializeField] public List<ClassObject> classList = new List<ClassObject>();
    [SerializeField] private ToolRegistry toolRegistry;

    public List<ClassObject> GetClasses()
    {
        return classList;
    }

    public List<ClassObject> GetClassesOfResource(ResourceNode.Resource resource)
    {
        List<ClassObject> classList = new List<ClassObject>();
        foreach (ClassObject classObject in classList)
        {
            if (classObject.targetResource == resource)
            {
                classList.Add(classObject);
            }
        }

        return classList;
    }
    
    public ClassObject GetClass(int index)
    {
        return classList[index];
    }
    
    public List<Tool> GetToolsOfClass(ClassObject classObject)
    {
        return toolRegistry.GetToolsOfResource(classObject.targetResource);
    }
    
    public List<Tool> GetToolsOfClass(int index)
    {
        return toolRegistry.GetToolsOfResource(classList[index].targetResource);
    }
}
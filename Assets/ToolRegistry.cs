using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolRegistry", menuName = "ToolRegistry")]
public class ToolRegistry : ScriptableObject
{
    public List<Tool> toolList = new List<Tool>();
    
    public List<Tool> GetToolsOfResource(ResourceNode.Resource resource)
    {
        List<Tool> tools = new List<Tool>();
        foreach (Tool tool in toolList)
        {
            if (tool.targetResource == resource)
            {
                tools.Add(tool);
            }
        }

        return tools;
    }
}
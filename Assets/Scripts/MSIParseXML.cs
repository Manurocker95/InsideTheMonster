using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MSIParseXML
{
    private Dictionary<string, string> tutorialList = new Dictionary<string, string>();


    public string getDescriptionByTriggerName(string TriggerName)
    {
        if(tutorialList.ContainsKey(TriggerName))
        {
            return tutorialList[TriggerName];
        }
        else
        {
            Debug.LogError("MSIParseXML Error! " + TriggerName + " doesn't exist in the parse dictionary");
        }
        return null;
    }

    public MSIParseXML(TextAsset script)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(script.text);
        XmlNodeList globalTable = xmlDoc.GetElementsByTagName("Table1");

        foreach (XmlNode tableNode in globalTable)
        {
            tutorialList.Add(tableNode.ChildNodes[1].InnerText, tableNode.ChildNodes[2].InnerText);
        }
    }
}

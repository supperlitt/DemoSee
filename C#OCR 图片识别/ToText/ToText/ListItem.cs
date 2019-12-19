using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ListItem
{
    public string _sValue { get; set; }
    public string _sText { get; set; }
    public ListItem()
    {
        _sValue = "";
        _sText = "";
    }

    public ListItem(string sText, string sValue)
    {
        _sValue = sValue;
        _sText = sText;
    }


}


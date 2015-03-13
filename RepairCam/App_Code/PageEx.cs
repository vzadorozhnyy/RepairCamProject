using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
/// <summary>
/// Summary description for PageEx
/// </summary>
public class PageEx: Page
{
    public delegate void DialogHandler(string dialogName, EventArgs e);

    public event DialogHandler CloseDialog;

    public void ProcessCloseDialog(string dialogName, EventArgs e)
    {
        if (null != CloseDialog)
        {
            CloseDialog(dialogName, e);
        }
    }
}
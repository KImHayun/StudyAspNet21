﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirstWebApp
{
    public partial class FrmResponseWrite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("안녕!<br />"); 
        }

        protected void BtnMsg_Click(object sender, EventArgs e)
        {
            Response.Write("<p span style ='color:pink;'>반가워!</p>");
        }

        protected void BtnJs_Click(object sender, EventArgs e)
        {
            string strScript = @"<script language ='javascript'>
                                window.alert ('안녕^-^*')</script>";
            Response.Write(strScript);
        }

        protected void BtnLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://www.naver.com/");
        }
    }
}
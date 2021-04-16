using DotNetNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNote.Board
{
    public partial class BoardView : System.Web.UI.Page
    {
        private string _Id; //현재 게시글 번호
        private string _Mode; //뷰에서 넘겨주는 모드 값


        protected void Page_Load(object sender, EventArgs e)
        {
            lnkDelete.NavigateUrl = $"BoardDelete.aspx?Id={Request["ID"]}";
            lnkModify.NavigateUrl = $"BoardWrite.aspx?Id={ Request["ID"]}&Mode=Edit";
            lnkReply.NavigateUrl = $"BoardWrite.aspx?Id={Request["ID"]}&Mode=Reply";

            _Id = Request["Id"]; //Get, POST 모두받음
            _Mode = Request["Mode"]; //Edit
            if (_Id == null) Response.Redirect("BoardList.aspx");
            if (!Page.IsPostBack) //게시판 글 내용 나옴
            {
                DisplayData();
            }
            

        }

        private void DisplayData()
        {
            var repo = new DbRepository();
            Note note = repo.GetNoteById(Convert.ToInt32(_Id));

            lblNum.Text = _Id;
            lblName.Text = note.Name;
            lblEmail.Text = string.Format("<a href='mailto:{0}'>{0}</a>", note.Email);
            lblTitle.Text = note.Title;

            string content = note.Content;

            //인코딩방식

            string encoding = note.Encoding;
            if (encoding == "Text")
            {
                lblContent.Text = Helpers.HtmlUtility.EncodeWithTabAndSpace(content);
            }
            else if (encoding == "Mixed")
            {
                lblContent.Text = content.Replace("\r\n", "</br>");
            }
            else //HTML
            {
                lblContent.Text = content; // HTML변화없음!
            }
            lblReadCount.Text = note.ReadCount.ToString();
            lblHomepage.Text = $"<a href = '{note.Homepage}' target='_blank'>{note.Homepage}</a>";
            lblPostDate.Text = note.PostDate.ToString();
            lblPostIP.Text = note.PostIp;

            if (note.FileName.Length >1 )
            {
                lblFile.Text = $"{note.FileName} /다운로드 {note.DownCount}";
            }
            else
            {
                lblFile.Text = "(None)";
            }

        }
    }
}
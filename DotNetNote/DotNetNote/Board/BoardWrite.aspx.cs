using DotNetNote.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNote.Board
{
    public partial class BoardWrite : System.Web.UI.Page

    {
        public BoardWriteFormType FormType { get; set; }

        private string _Id; //리스트에서 넘겨주는 번호
                            // private string _Mode; // 뷰에서 넘겨주는 모드값

        private string _BaseDir = "";
        private string _FileName = "";
        private int _FileSize = 0; //파일사이즈

        protected void Page_Load(object sender, EventArgs e)
        {
            _Id = Request["Id"];
            //_Mode = Request["Mode"];


            if (!Page.IsPostBack)
            {
                ViewState["Mode"] = Request["Mode"]; //Edit
                if (ViewState["Mode"].ToString() == "Edit") FormType = BoardWriteFormType.Modify;
                else if (ViewState["Mode"].ToString() == "Reply") FormType = BoardWriteFormType.Reply;
                else FormType = BoardWriteFormType.Write;

                switch (FormType)
                {
                    case BoardWriteFormType.Write:
                        LblTitleDescript.Text = "글쓰기 - 다음 필드를 입력하세요";
                        break;

                    case BoardWriteFormType.Modify:
                        LblTitleDescript.Text = "글수정 - 아래 필드들을 수정하세요";
                        DisplayDataForModify();
                        break;


                    case BoardWriteFormType.Reply:
                        
                        LblTitleDescript.Text = "글답변 - 다음 필드들을 입력하세요";
                        DisplayDataForReply();
                        break;
                }
            }
        }
        private void DisplayDataForModify()
        {
            var repo = new DbRepository();
            Note note = repo.GetNoteById(Convert.ToInt32(_Id));

            txtName.Text = note.Name;
            txtEmail.Text = note.Email;
            txtHomepage.Text = note.Homepage;
            txtTitle.Text = note.Title;
            txtContent.Text = note.Content;

            //Encoding
            string encoding = note.Encoding;
            if (encoding == "Text") rdoEncoding.SelectedIndex = 0;
            else if (encoding == "Mixed") rdoEncoding.SelectedIndex = 2;
            else rdoEncoding.SelectedIndex = 1;

            //파일처리
           
        }

        private void DisplayDataForReply()
        {
            var repo = new DbRepository();
            Note note = repo.GetNoteById(Convert.ToInt32(_Id));

            txtTitle.Text = $"답변 : {note.Title}";
            txtContent.Text = $"\n\n작성일: {note.PostDate}, 작성자 : '{note.Name}'\n--------------------\n>" +
                $"{note.Content.Replace("\n", "\n")}\n-----------------------------\n";

        }


        protected void chkUpload_CheckedChanged(object sender, EventArgs e)
        {
            pnlFile.Visible = !pnlFile.Visible; //답변 파일
        }


        protected void btnWrite_Click(object sender, EventArgs e)
        {
            if (IsImageTextCorrect())
            {
                if (ViewState["Mode"].ToString() == "Edit") FormType = BoardWriteFormType.Modify;
                else if (ViewState["Mode"].ToString() == "Reply") FormType = BoardWriteFormType.Reply;
                else FormType = BoardWriteFormType.Write;

                UploadFile(); //파일업로드

                Note note = new Note();
                note.Id = Convert.ToInt32(_Id); //값이 없으면 0
                note.Name = txtName.Text; //필수
                note.Email = txtEmail.Text;
                note.Title = txtTitle.Text; //필수
                note.Homepage = txtHomepage.Text;
                note.Content = txtContent.Text; //필수
                note.FileName = _FileName;
                note.FileSize = _FileSize;
                note.Password = txtPassword.Text;
                note.PostIp = Request.UserHostAddress;
                note.Encoding = rdoEncoding.SelectedValue; // Text, Html, Mixed

                DbRepository repo = new DbRepository();
                switch (FormType)
                {
                    case BoardWriteFormType.Write:
                        repo.Add(note);
                        Response.Redirect("BoardList.aspx");
                        break;

                    case BoardWriteFormType.Modify:
                        note.ModifyIp = Request.UserHostAddress;
                        //파일처리
                        note.FileName = ViewState["FileName"].ToString();
                        note.FileSize = Convert.ToInt32(ViewState["FileSize"]);
                        if (repo.UpdateNote(note) > 0) Response.Redirect($"BoardView.aspx?Id={_Id}");
                        else lblError.Text = "업데이트 실패, 암호를 확인하세요.";
                        break;
                    case BoardWriteFormType.Reply:
                        note.ParentNum = Convert.ToInt32(_Id);
                        repo.ReplyNote(note);//답변내용저장
                        Response.Redirect("BoardList.aspx");
                        break;
                    default:
                        repo.Add(note);
                        Response.Redirect("BoardList.aspx");
                        break;
                }
            }
            else
            {
                lblError.Text = "보안코드가 틀립니다. 다시 입력하세요.";
            }
        }

        private void UploadFile()
        {
            _BaseDir = Server.MapPath("../Files");
            _FileName = "";
            _FileSize = 0;

            if (txtFileName.PostedFile != null)
            {
                if (txtFileName.PostedFile.FileName.Trim().Length > 0 &&
                    txtFileName.PostedFile.ContentLength>0)
                {
                    if (FormType == BoardWriteFormType.Modify) //수정일 경우만
                    {
                        ViewState["FileName"] = Helpers.FileUtility.GetFileNameWithNumbering(_BaseDir, Path.GetFileName(txtFileName.PostedFile.FileName));
                        ViewState["FileName"] = txtFileName.PostedFile.ContentLength;
                        //업로드
                        txtFileName.PostedFile.SaveAs(Path.Combine(_BaseDir, _FileName));

                    }
                    else  //Write, Reply
                    {    
                        //폴더 이미 test.txt 있으면 test(1).txt로 변경
                        _FileName = Helpers.FileUtility.GetFileNameWithNumbering(_BaseDir, Path.GetFileName(txtFileName.PostedFile.FileName));
                        _FileSize = txtFileName.PostedFile.ContentLength;
                        //업로드
                        txtFileName.PostedFile.SaveAs(Path.Combine(_BaseDir, _FileName));
                    }
                }
            }
        }

        private bool IsImageTextCorrect()
        {
            if (Page.User.Identity.IsAuthenticated) //이미 로그인 했으면
            {
                return true;
            }
            else
            {
                if (Session["ImageText"] != null)
                {
                    return (txtImageText.Text == Session["ImageText"].ToString());
                }
            }
            return false; //보안코드 불일치!
        }
    }
}
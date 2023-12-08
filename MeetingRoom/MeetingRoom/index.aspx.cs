using System;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace MEETING
{
    public partial class Index : System.Web.UI.Page
    {
        string connectionString = @"Server=localhost;Database=meetingroom_db;Uid=root;Pwd=VueData@2023;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Clear();
                GridFill();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
                {
                    sqlCon.Open();

                    DateTime startTime = DateTime.ParseExact(txtStartTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null);
                    DateTime endTime = DateTime.ParseExact(txtEndTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null);

                    if (endTime <= startTime)
                    {
                        lblSuccessMessage.Text = "";
                        lblErrorMessage.Text = "End time must be after start time.";
                        return;
                    }

                    MySqlCommand checkOrganizerCmd = new MySqlCommand("SELECT COUNT(*) FROM employee WHERE EmployeeId = @OrganizerId", sqlCon);
                    checkOrganizerCmd.Parameters.AddWithValue("@OrganizerId", Convert.ToInt32(txtOrganizerId.Text.Trim()));

                    int organizerCount = Convert.ToInt32(checkOrganizerCmd.ExecuteScalar());

                    if (organizerCount == 0)
                    {
                        lblSuccessMessage.Text = "";
                        lblErrorMessage.Text = "OrganizerId not available.";
                    }
                    else
                    {
                        MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM meeting WHERE RoomId = @RoomId AND " +
                            "((StartTime <= @StartTime AND EndTime >= @StartTime) OR (StartTime <= @EndTime AND EndTime >= @EndTime))" +
                            "AND MeetingId != @MeetingId", sqlCon);
                        checkCmd.Parameters.AddWithValue("@StartTime", DateTime.ParseExact(txtStartTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                        checkCmd.Parameters.AddWithValue("@EndTime", DateTime.ParseExact(txtEndTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                        checkCmd.Parameters.AddWithValue("@RoomId", Convert.ToInt32(ddlRoomId.Text.Trim()));
                        checkCmd.Parameters.AddWithValue("@MeetingId", Convert.ToInt32(hfMeetingId.Value == "" ? "0" : hfMeetingId.Value));

                        int existingMeetingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existingMeetingCount > 0)
                        {
                            lblSuccessMessage.Text = "";
                            lblErrorMessage.Text = "Meeting time slot is already booked.";
                        }
                        else
                        {
                            MySqlCommand sqlCmd = new MySqlCommand("MeetingAddorEdit", sqlCon);
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.AddWithValue("p_MeetingId", Convert.ToInt32(hfMeetingId.Value == "" ? "0" : hfMeetingId.Value));
                            sqlCmd.Parameters.AddWithValue("p_StartTime", DateTime.ParseExact(txtStartTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                            sqlCmd.Parameters.AddWithValue("p_EndTime", DateTime.ParseExact(txtEndTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                            sqlCmd.Parameters.AddWithValue("p_OrganizerId", Convert.ToInt32(txtOrganizerId.Text.Trim()));
                            sqlCmd.Parameters.AddWithValue("p_RoomId", Convert.ToInt32(ddlRoomId.Text.Trim()));
                            sqlCmd.ExecuteNonQuery();
                            GridFill();
                            Clear();
                            lblSuccessMessage.Text = "Submitted Successfully";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }

        void Clear()
        {
            hfMeetingId.Value = "";
            txtStartTime.Text = txtEndTime.Text = txtOrganizerId.Text = ddlRoomId.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            lblErrorMessage.Text = lblSuccessMessage.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void GridFill()
        {
            using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
            {
                sqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("MeetingViewAll", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                gvMeeting.DataSource = dtbl;
                gvMeeting.DataBind();
            }
        }

        protected void lnkSelect_OnClick(object sender, EventArgs e)
        {
            int meetingId = Convert.ToInt32((sender as LinkButton).CommandArgument);
            using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("MeetingViewByID", sqlCon);
                sqlCmd.Parameters.AddWithValue("p_MeetingId", meetingId);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.Read())
                {
                    DateTime startTime = Convert.ToDateTime(reader["StartTime"]);
                    DateTime endTime = Convert.ToDateTime(reader["EndTime"]);

                    txtStartTime.Text = startTime.ToString("yyyy-MM-ddTHH:mm");
                    txtEndTime.Text = endTime.ToString("yyyy-MM-ddTHH:mm");

                    txtOrganizerId.Text = reader["OrganizerId"].ToString();
                    ddlRoomId.Text = reader["RoomId"].ToString();

                    hfMeetingId.Value = reader["MeetingId"].ToString();

                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
                {
                    sqlCon.Open();
                    MySqlCommand sqlCmd = new MySqlCommand("MeetingAddorEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("p_MeetingId", Convert.ToInt32(hfMeetingId.Value == "" ? "0" : hfMeetingId.Value));
                    sqlCmd.Parameters.AddWithValue("p_StartTime", DateTime.ParseExact(txtStartTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                    sqlCmd.Parameters.AddWithValue("p_EndTime", DateTime.ParseExact(txtEndTime.Text.Trim(), "yyyy-MM-ddTHH:mm", null));
                    sqlCmd.Parameters.AddWithValue("p_OrganizerId", Convert.ToInt32(txtOrganizerId.Text.Trim()));
                    sqlCmd.Parameters.AddWithValue("p_RoomId", Convert.ToInt32(ddlRoomId.Text.Trim()));
                    sqlCmd.ExecuteNonQuery();
                    GridFill();
                    Clear();
                    lblSuccessMessage.Text = "Submitted Successfully";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("MeetingDeleteByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("p_MeetingId", Convert.ToInt32(hfMeetingId.Value == "" ? "0" : hfMeetingId.Value));
                sqlCmd.ExecuteNonQuery();
                GridFill();
                Clear();
                lblSuccessMessage.Text = "Deleted Successfully";
            }
        }
    }
}

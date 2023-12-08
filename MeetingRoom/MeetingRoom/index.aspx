<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MEETING.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Meeting Management</title>
    <link rel="stylesheet" type="text/css" href="Styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hfMeetingId" runat="server" />

            <h1>Meeting Details</h1>    
            
            <table id="input">
              
                <tr>
                    <td>
                        <asp:Label ID="lblStartTime" Text="Start Time:" runat="server" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtStartTime" runat="server" type="datetime-local" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEndTime" Text="End Time:" runat="server" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtEndTime" runat="server" type="datetime-local" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblOrganizerId" Text="Organizer Id:" runat="server" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtOrganizerId" runat="server" />
                    </td>
                </tr>
                <tr>
                     <td>
                         <asp:Label ID="lblRoomId" Text="Room Id:" runat="server" /> </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlRoomId" runat="server">
                         <asp:ListItem Text="Select a Room" Value="" /> 
                        <asp:ListItem Text="Inspiration Chamber" Value="1" />
                        <asp:ListItem Text="War Room" Value="2" />
                        <asp:ListItem Text="Cubical Room" Value="3" />

                        </asp:DropDownList >

                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2" style="padding-left:20%">
                        <asp:Button ID="btnSave" runat="server" Text="Insert" OnClick="btnSave_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <br />
            <h1>Meeting List</h1>
            <asp:GridView ID="gvMeeting" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="MeetingId" HeaderText="Meeting Number" />
                    <asp:BoundField DataField="StartTime" HeaderText="Start Time" />
                    <asp:BoundField DataField="EndTime" HeaderText="End Time" />
                    <asp:BoundField DataField="OrganizerName" HeaderText="Organizer Name" />
                    <asp:BoundField DataField="Roomname" HeaderText="Room name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton Text="Select" ID="lnkSelect" CommandArgument='<%# Eval("MeetingId") %>' runat="server" OnClick="lnkSelect_OnClick" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>

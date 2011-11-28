<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EventStackView.ascx.cs"
    Inherits="EventStackView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="Sol" TagName="MessageBox" Src="~/MessageBox.ascx" %>
<link href="../css/EventStack.css" rel="stylesheet" type="text/css" />
<%--<link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.12.custom.css" rel="stylesheet" /> --%>
<style type="text/css">
    .ui-widget
    {
        font-size: 0.9em;
        margin-bottom: 4px;
    }
    .ui-button
    {
        margin-left: -1px;
    }
    .ui-button-icon-only .ui-button-text
    {
        padding: 0.35em;
    }
    .ui-autocomplete-input
    {
        margin: 0;
        padding: 0.48em 0 0.47em 0.45em;
    }
    .ui-autocomplete
    {
        max-height: 100px;
        overflow-y: auto; /* prevent horizontal scrollbar */
        overflow-x: hidden; /* add padding to account for vertical scrollbar */
        padding-right: 20px;
    }
</style>
<script type="text/javascript">
    (function ($) {
        $.widget("ui.combobox", {
            _create: function () {
                var self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";
                var input = this.input = $("<input>")
					.insertAfter(select)
					.val(value)
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function (request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function (event, ui) {
					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					    },
					    change: function (event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;
					            select.children("option").each(function () {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    return false;
					                }
					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

                input.data("autocomplete")._renderItem = function (ul, item) {
                    return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
                };

                this.button = $("<button type='button'>&nbsp;</button>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter(input)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon")
					.click(function () {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
            },

            destroy: function () {
                this.input.remove();
                this.button.remove();
                this.element.show();
                $.Widget.prototype.destroy.call(this);
            }
        });
    })(jQuery);

    function pageLoad(sender, args) {
        $(function () {
            $('[id*="comboEventClient"]').combobox();
            $('[id*="comboEventType"]').combobox();
            $('[id*="comboEventAssignedUser"]').combobox();
            var options = {};
            $('.divEventHeader').click(function () {
                $(this).next().toggle('blind', options, 'fast');
            });
        });

        $('.container').fadeTo(5000, 0.5, function () { $(this).hide(); });
        $('[id*="txtMessage"]').html("");
    }

    function OnEventStatusChange(status, eventID) {
        if (status == "" || eventID == "")
            return false;
        //alert(event + " for event " + eventID);

        $('[id*="hdnEventStatusID"]').val(status);
        $('[id*="hdnEventStatusValue"]').val(eventID);
        $('[id*="hdnDoAsyncPostback"]').click();

        return true;
    }
    function OnDueEventMouseOver(event) {
        var targetid = "unknown";
        if (event.target.id) {
            targetid = event.target.id.toString()
        }
        else {
            targetid = event.relatedTarget.id.toString()
        }
        document.getElementById('debugger1').innerHTML = "over " + targetid;
    }
    function OnDueEventMouseOut(event) {
        var targetid = "unknown";
        if (event.target.id) {
            targetid = event.target.id.toString()
        }
        else {
            //targetid = event.relatedTarget.id.toString()
            return;
        }
        document.getElementById('debugger2').innerHTML = "out " + targetid;
    }

    function SelectUser(user) {
        $('[id*="hdnSelectedUser"]').val(user);
        $('[id*="hdnUserSelected"]').click();
        return true;
    }

</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ViewStateMode="Enabled"
    EnableViewState="true">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSubmitNote" />
        <asp:AsyncPostBackTrigger ControlID="hdnDoAsyncPostback" />        
        <asp:AsyncPostBackTrigger ControlID="hdnUserSelected" />
    </Triggers>
    <ContentTemplate>
        <asp:HiddenField ID="hdnEventStatusID" runat="server" />
        <asp:HiddenField ID="hdnEventStatusValue" runat="server" />        
        <asp:HiddenField ID="hdnSelectedUser" runat="server" />
        <asp:Button ID="hdnUserSelected" runat="server" Text="Button" 
            Style="display: none;" OnClick="hdnUserSelected_Click" />
        <asp:Button ID="hdnDoAsyncPostback" runat="server" Text="Button" Style="display: none;"
            OnClick="hdnDoAsyncPostback_Click" />
        <div id="divEventStackView" class="EventStack">
            <h3 class="divEventHeader" id="headerAddEvent" runat="server">
                Add an Event
            </h3>
            <div class="UserEvent" id="divAddNewEvent" runat="server">
                <Sol:MessageBox runat="server" ShowCloseButton="true" ID="msgAddEvent" />
                <div class="ui-widget">
                    <label style="float: left; width: 100px; padding: 5px;">
                        Assigned To:
                    </label>
                    <table id="tblMarketers" runat="server">
                    </table>
                </div>
                <div class="ui-widget">
                    <label style="float: left; width: 100px; padding: 5px;">
                        Client:
                    </label>
                    <select id="comboEventClient" runat="server">
                    </select>
                </div>
                <div class="ui-widget">
                    <label style="float: left; width: 100px; padding: 5px;">
                        Type:
                    </label>
                    <select id="comboEventType" runat="server">
                    </select>
                </div>
                <div>
                    <label style="float: left; width: 100px; padding: 5px;">
                        Due date:
                    </label>
                    <asp:TextBox ID="txtEventDate" runat="server" MaxLength="1" class="textboxBorder" Height="25px"
                        Width="130px" ValidationGroup="EventDateValidation"></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtEventDate_MaskedEditExtender" runat="server" CultureName="en-GB"
                        MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true" MessageValidatorTip="true"
                        ClearMaskOnLostFocus="false" TargetControlID="txtEventDate" AutoComplete="true" />
                    <input type="image" style="margin-top: 3px;" id="btnDatePicker" name="btnDatePicker"
                        src="../images/icons/dp_button.png" alt="Click to show date picker" />
                    <asp:MaskedEditValidator ID="txtEventDate_MaskedEditValidator" Style="padding: 10px"
                        CultureName="en-GB" runat="server" IsValidEmpty="false" Display="Dynamic" SetFocusOnError="true"
                        ForeColor="Red" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                        TooltipMessage="Input a date" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*"
                        ControlExtender="txtEventDate_MaskedEditExtender" ControlToValidate="txtEventDate"
                        ValidationGroup="EventDateValidation" />
                    <asp:CalendarExtender ID="txtEventDate_CalendarExtender" runat="server" Enabled="True"
                        TodaysDateFormat="dd/MM/yyyy" PopupButtonID="btnDatePicker" TargetControlID="txtEventDate"
                        Format="dd/MM/yyyy" Animated="true">
                    </asp:CalendarExtender>
                </div>
                <div style="clear: both; padding-top: 5px;">
                    <label style="float: left; width: 100px;  padding: 5px;">
                        Due time:
                    </label>
                    <asp:TextBox ID="txtEventTime" runat="server" MaxLength="1" class="textboxBorder" Height="25px"
                        Width="80px" ValidationGroup="EventDateValidation"></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtEventTime_MaskedEditExtender" runat="server" CultureName="en-GB"
                        MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true" MessageValidatorTip="true"
                        ClearMaskOnLostFocus="false" TargetControlID="txtEventTime" AcceptAMPM="false"
                        UserTimeFormat="TwentyFourHour" AutoComplete="true" />
                    <asp:MaskedEditValidator ID="txtEventTime_MaskedEditValidator" Style="padding: 10px"
                        CultureName="en-GB" runat="server" IsValidEmpty="false" Display="Dynamic" SetFocusOnError="true"
                        ForeColor="Red" EmptyValueMessage="Time is required" InvalidValueMessage="Time is invalid"
                        TooltipMessage="Input a time" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*"
                        ControlExtender="txtEventTime_MaskedEditExtender" ControlToValidate="txtEventTime"
                        ValidationGroup="EventDateValidation" />
                </div>
                <div style="clear: both; padding-top: 5px;">
                    <label style="float: left; width: 100px;  padding: 5px; padding-top: 5px; padding-bottom: 5px;">
                        Reminder SMS:
                    </label>
                    <asp:TextBox ID="txtReminderTime" runat="server" MaxLength="1" class="textboxBorder" Height="25px"
                        Width="80px" ValidationGroup="EventDateValidation"></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtReminderTime_MaskedEditExtender" runat="server" CultureName="en-GB"
                        MaskType="Time" Mask="99:99" ErrorTooltipEnabled="true" MessageValidatorTip="true"
                        ClearMaskOnLostFocus="false" TargetControlID="txtReminderTime" AcceptAMPM="false"
                        UserTimeFormat="TwentyFourHour" AutoComplete="true" />
                    <asp:CheckBox ID="cbReminderSMS" runat="server" TextAlign="left" Text="" Checked="false" />
                    <asp:MaskedEditValidator ID="txtReminderTime_MaskedEditValidator" Style="padding: 10px"
                        CultureName="en-GB" runat="server" IsValidEmpty="false" Display="Dynamic" SetFocusOnError="true"
                        ForeColor="Red" EmptyValueMessage="Time is required" InvalidValueMessage="Time is invalid"
                        TooltipMessage="Input a time" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*"
                        ControlExtender="txtReminderTime_MaskedEditExtender" ControlToValidate="txtReminderTime"
                        ValidationGroup="EventDateValidation" />
                </div>
                <div class="EventContent">
                    <span></span>
                    <textarea id="txtMessage" runat="server" rows="10" cols="48" style="resize: none;"></textarea>
                </div>
                <div style="padding-bottom: 20px;">
                    <asp:Button runat="server" Text="Submit" ID="btnSubmitNote" Style="float: right;
                        margin: 5px;" OnClick="btnSubmitNote_Click" ValidationGroup="EventDateValidation"
                        CausesValidation="true" />
                    <asp:ValidationSummary Style="padding: 10px;" ForeColor="Red" ID="ValidationSummary1"
                        runat="server" ValidationGroup="EventDateValidation" />
                </div>
            </div>
            <h2 class="divEventHeader" id="headerDueEvent" runat="server">
                Due Events
            </h2>
            <div id="divDueEvents" runat="server">
                <p id="debugger1">
                </p>
                <p id="debugger2">
                </p>
                <table runat="server" id="tblDueEvents" cellpadding="0" cellspacing="0" border="1"
                    style="border-style: solid; border-color: #ff4747; margin-top: 0px; margin-bottom: 5px;
                    width: 100%;">
                </table>
            </div>
            <h2 class="divEventHeader" id="headerEvents" runat="server">
                Events
            </h2>
            <div id="divEventStack" class="UserEvent" runat="server">
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

﻿// ****************************************************************************************
// Assembly         : Microsoft.Legal.MatterCenter.Selenium
// Author           : MAQ Software
// Created          : 11-09-2016
//
// ***********************************************************************
// <copyright file="Settings.cs" company="Microsoft">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>This file is used to perform verification of settings page </summary>
// ****************************************************************************************

namespace Microsoft.Legal.MatterCenter.Selenium
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using TechTalk.SpecFlow;

    [Binding]
    public class Settings
    {
        string URL = ConfigurationManager.AppSettings["Settings"],
               createURL = ConfigurationManager.AppSettings["CreateMatter"];
        static IWebDriver webDriver = CommonHelperFunction.GetDriver();
        IJavaScriptExecutor scriptExecutor = (IJavaScriptExecutor)webDriver;
        CommonHelperFunction common = new CommonHelperFunction();
        CultureInfo culture = CultureInfo.CurrentCulture;

        #region 01. Open the browser and load settings page
        [When(@"user enters credentials on settings page")]
        public void WhenUserEntersCredentialsOnSettingsPage()
        {
            common.GetLogin(webDriver, URL);
        }

        [Then(@"settings page should be loaded with element '(.*)'")]
        public void ThenSettingsPageShouldBeLoadedWithElement(string settingsName)
        {
            Assert.IsTrue(common.ElementPresent(webDriver, settingsName, Selector.Id));
        }
        #endregion

        #region 02. Verify deletion of team members 

        [When(@"user tries to delete all the team members")]
        public void WhenUserTriesToDeleteAllTheTeamMembers()
        {
            common.GetLogin(webDriver, URL);
            Thread.Sleep(2000);
            int noOfMembers = Convert.ToInt32(scriptExecutor.ExecuteScript("var length = $('.assignNewPerm').length; return length;"));

            for (int index = 0; index < noOfMembers; index++)
            {
                scriptExecutor.ExecuteScript("$('.close')[0].click()");
                Thread.Sleep(1000);
            }
        }

        [Then(@"last team member should not be deleted")]
        public void ThenLastTeamMemberShouldNotBeDeleted()
        {
            int noOfMembers = Convert.ToInt32(scriptExecutor.ExecuteScript("var length = $('.assignNewPerm').length; return length;"));
            Assert.IsTrue(noOfMembers > 0);
        }

        #endregion

        #region 03. Verify error message on adding non-existing team member

        [When(@"user adds non-existing Attorney to the team")]
        public void WhenUserAddsNon_ExistingAttorneyToTheTeam()
        {
            common.GetLogin(webDriver, URL);
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).Click();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).Clear();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).SendKeys(ConfigurationManager.AppSettings["MatterName"]);
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).Click();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).Clear();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).SendKeys(ConfigurationManager.AppSettings["MatterDescription"]);
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('.popUpPGDiv').click()");
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('.popUpOptions')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("img.iconForward.iconPosition")).Click();
            Thread.Sleep(3000);
            scriptExecutor.ExecuteScript("$('.popUpDTContent')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("div.popUpDTContent.popUpSelected")).Click();
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('#assignTeamTrue')[0].click()");
            scriptExecutor.ExecuteScript("$('#assignTeamFalse')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("txtAssign1")).Click();
            webDriver.FindElement(By.Id("txtAssign1")).Clear();
            webDriver.FindElement(By.Id("txtAssign1")).SendKeys(ConfigurationManager.AppSettings["Gibberish"]);
            scriptExecutor.ExecuteScript("$('.ui-menu-item')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("ddlRoleAssignIcon1")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("//div[@id='ddlRoleAssignList1']/div[4]")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("ddlPermAssignIcon1")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("//div[@id='ddlPermAssignList1']/div[2]")).Click();
            Thread.Sleep(3000);
            scriptExecutor.ExecuteScript("$('#includeRSSTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeEmailTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeCalendarTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeTasksTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#matterRequiredTrue').click()");
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('#saveButton').click()");
            Thread.Sleep(3000);
        }

        [Then(@"error should be thrown on saving")]
        public void ThenErrorShouldBeThrownOnSaving()
        {
            string errorMsg = (string)scriptExecutor.ExecuteScript("var error = $('.errText')[0].innerText; return error;");
            Assert.IsTrue(errorMsg.ToLower(CultureInfo.CurrentCulture).Contains("please enter valid team members"));
        }

        #endregion

        #region  02. Set the value on settings page 
        [When(@"settings page is configured and save button is clicked")]
        public void WhenSettingsPageIsConfiguredAndSaveButtonIsClicked()
        {
            common.GetLogin(webDriver, URL);
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).Click();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).Clear();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterName")).SendKeys(ConfigurationManager.AppSettings["MatterName"]);
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).Click();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).Clear();
            webDriver.FindElement(By.CssSelector("input.ms-TextField-field.inputMatterId")).SendKeys(ConfigurationManager.AppSettings["MatterDescription"]);
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('.popUpPGDiv').click()");
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('.popUpOptions')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("img.iconForward.iconPosition")).Click();
            Thread.Sleep(3000);
            scriptExecutor.ExecuteScript("$('.popUpDTContent')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.CssSelector("div.popUpDTContent.popUpSelected")).Click();
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('#assignTeamTrue')[0].click()");
            scriptExecutor.ExecuteScript("$('#assignTeamFalse')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("txtAssign1")).Click();
            webDriver.FindElement(By.Id("txtAssign1")).Clear();
            webDriver.FindElement(By.Id("txtAssign1")).SendKeys(ConfigurationManager.AppSettings["AttorneyName"]);
            scriptExecutor.ExecuteScript("$('.ui-menu-item')[0].click()");
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("ddlRoleAssignIcon1")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("//div[@id='ddlRoleAssignList1']/div[4]")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("ddlPermAssignIcon1")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("//div[@id='ddlPermAssignList1']/div[2]")).Click();
            Thread.Sleep(3000);
            scriptExecutor.ExecuteScript("$('#includeRSSTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeEmailTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeCalendarTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#includeTasksTrue').click()");
            Thread.Sleep(1000);
            scriptExecutor.ExecuteScript("$('#matterRequiredTrue').click()");
            Thread.Sleep(2000);
            scriptExecutor.ExecuteScript("$('#saveButton').click()");
            Thread.Sleep(3000);
        }

        [Then(@"settings should be saved and confirmation message should be displayed")]
        public void ThenSettingsShouldBeSavedAndConfirmationMessageShouldBeDisplayed()
        {
            string successMessage = (string)scriptExecutor.ExecuteScript("var links = $('#successMessage')[0].innerText;return links"),
                   clientLink = (string)scriptExecutor.ExecuteScript("var links = $('.clientLinks').attr('href');return links"),
                   pageDescription = (string)scriptExecutor.ExecuteScript("var links = $('.pageDescription')[0].innerText;return links");
            Assert.IsTrue(successMessage.ToLower(CultureInfo.CurrentCulture).Contains("your changes have been saved. go back to clients"));
            Assert.IsTrue(clientLink.ToLower(CultureInfo.CurrentCulture).Contains("https://msmatter.sharepoint.com/sitepages/settings.aspx"));
            Assert.IsTrue(pageDescription.ToLower(CultureInfo.CurrentCulture).Contains("this page shows the current settings for this client’s new matters. the first section allows you to set new matter default selections, which can be changed when a matter is created. the second section defines settings that can not be changed when a new matter is created. no changes are required, and any changes made will not affect existing matters"));
        }
        #endregion     

        #region 03. Verify values on matter provision page 

        [When(@"user goes to matter provision page")]
        public void WhenUserGoesToMatterProvisionPage()
        {
            common.GetLogin(webDriver, createURL);
            Thread.Sleep(5000);
            webDriver.FindElement(By.XPath("//main/div/div/div")).Click();
            Thread.Sleep(2000);
            webDriver.FindElement(By.XPath("//section[@id='snOpenMatter']/div/div[2]/select")).Click();
            Thread.Sleep(3000);
            new SelectElement(webDriver.FindElement(By.XPath("//section[@id='snOpenMatter']/div/div[2]/select"))).SelectByText(ConfigurationManager.AppSettings["DropDownKeyword"]);
            new SelectElement(webDriver.FindElement(By.XPath("//section[@id='snOpenMatter']/div/div[2]/select"))).SelectByText(ConfigurationManager.AppSettings["DropDownClient"]);
            Thread.Sleep(2000);
            webDriver.FindElement(By.Id("txtMatterDesc")).Clear();
            webDriver.FindElement(By.Id("txtMatterDesc")).SendKeys(ConfigurationManager.AppSettings["MatterDescription"]);
            Thread.Sleep(2000);

        }

        [Then(@"preset values should be loaded")]
        public void ThenPresetValuesShouldBeLoaded()
        {
            string matterName = (string)scriptExecutor.ExecuteScript("var mName = $('#txtMatterName')[0].value; return mName;"),
                   assignedTo = string.Empty;
            Assert.IsTrue(matterName.ToLower(CultureInfo.CurrentCulture).Contains(ConfigurationManager.AppSettings["MatterName"].ToLower(CultureInfo.CurrentCulture)));
            scriptExecutor.ExecuteScript("$('.buttonPrev')[1].click()");
            Thread.Sleep(3000);
            assignedTo = (string)scriptExecutor.ExecuteScript("var aName = $('.inputAssignPerm')[0].value; return aName;");
            Assert.IsTrue(assignedTo.ToLower(CultureInfo.CurrentCulture).Contains(ConfigurationManager.AppSettings["AttorneyName"].ToLower(CultureInfo.CurrentCulture)));
            webDriver.Quit();
        }

        #endregion

    }
}


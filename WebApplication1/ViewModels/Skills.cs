

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class Skills
    {
        public Boolean Ajax { get; set; }
        public Boolean AngularJS { get; set; }
        public Boolean AspectCPlusPlus { get; set; }
        public Boolean Assembly { get; set; }
        public Boolean AspdotNet { get; set; }
        public Boolean BASIC { get; set; }
        public Boolean C { get; set; }
        public Boolean CPlusPlus { get; set; }
        public Boolean CSharp { get; set; }
        public Boolean C_RIMM { get; set; }
        public Boolean CSS { get; set; }
        public Boolean Fortran { get; set; }
        public Boolean Java { get; set; }
        public Boolean Javascript { get; set; }
        public Boolean MySQL { get; set; }
        public Boolean NodeJS { get; set; }
        public Boolean ObjectiveC { get; set; }
        public Boolean Perl { get; set; }
        public Boolean PHP { get; set; }
        public Boolean Python { get; set; }
        public Boolean Ruby { get; set; }
        public Boolean SQLVariations { get; set; }
        public Boolean VisualBasic { get; set; }
        public Boolean XML { get; set; }
        public Boolean Laravel { get; set; }
        public Boolean AdobeSuite { get; set; }
        public Boolean JQuery { get; set; }
        public Boolean Json { get; set; }
        public Boolean TwitterBootStrap { get; set; }
        public Skills()
        {
            Ajax = false;
            AngularJS = false;
            AspectCPlusPlus = false;
            Assembly = false;
            AspdotNet = false;
            BASIC = false;
            C = false;
            CPlusPlus = false;
            CSharp = false;
            C_RIMM = false;
            CSS = false;
            Fortran = false;
            Java = false;
            Javascript = false;
            MySQL = false;
            NodeJS = false;
            ObjectiveC = false;
            Perl = false;
            PHP = false;
            Python = false;
            Ruby = false;
            SQLVariations = false;
            VisualBasic = false;
            XML = false;
            Laravel = false;
            AdobeSuite = false;
            JQuery = false;
            Json = false;
            TwitterBootStrap = false;
        }


        public Skills(Boolean ajax, Boolean angularJS, Boolean aspectCPlusPlus, Boolean assembly, Boolean aspdotNet, Boolean bASIC, Boolean c, Boolean cPlusPlus, Boolean cSharp,
                    Boolean c_RIMM, Boolean cSS, Boolean fortran, Boolean java, Boolean javascript, Boolean mySQL, Boolean nodeJS, Boolean objectiveC, Boolean perl,
                    Boolean pHP, Boolean python, Boolean ruby, Boolean sQLVariations, Boolean visualBasic, Boolean xML, Boolean laravel, Boolean adobeSuite, Boolean jQurey,
                    Boolean json, Boolean twitterBootStrap

            )
        {
            Ajax = ajax;
            AngularJS = angularJS;
            AspectCPlusPlus = aspectCPlusPlus;
            Assembly = assembly;
            AspdotNet = aspdotNet;
            BASIC = bASIC;
            C = c;
            CPlusPlus = cPlusPlus;
            CSharp = cSharp;
            C_RIMM = c_RIMM;
            CSS = cSS;
            Fortran = fortran;
            Java = java;
            Javascript = javascript;
            MySQL = mySQL;
            NodeJS = nodeJS;
            ObjectiveC = objectiveC;
            Perl = perl;
            PHP = pHP;
            Python = python;
            Ruby = ruby;
            SQLVariations = sQLVariations;
            VisualBasic = visualBasic;
            XML = xML;
            Laravel = laravel;
            AdobeSuite = adobeSuite;
            JQuery = jQurey;
            Json = json;
            TwitterBootStrap = twitterBootStrap;
        }
    }
}
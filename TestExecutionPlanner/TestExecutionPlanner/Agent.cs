using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExecutionPlanner
{

    public class Agent
    {
        public int count { get; set; }
        public ValueA[] value { get; set; }
    }

    public class ValueA
    {
        public Systemcapabilities systemCapabilities { get; set; }
        public Usercapabilities userCapabilities { get; set; }
        public _Links _links { get; set; }
        public int maxParallelism { get; set; }
        public DateTime createdOn { get; set; }
        public Authorization authorization { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string osDescription { get; set; }
        public bool enabled { get; set; }
        public string status { get; set; }
        public string provisioningState { get; set; }
        public string accessPoint { get; set; }
    }

    public class Systemcapabilities
    {
        public string AgentName { get; set; }
        public string AgentVersion { get; set; }
        public string AgentComputerName { get; set; }
        public string AgentHomeDirectory { get; set; }
        public string AgentOS { get; set; }
        public string AgentOSVersion { get; set; }
        public string ALLUSERSPROFILE { get; set; }
        public string APPDATA { get; set; }
        public string Cmd { get; set; }
        public string CommonProgramFiles { get; set; }
        public string CommonProgramFilesx86 { get; set; }
        public string CommonProgramW6432 { get; set; }
        public string COMPUTERNAME { get; set; }
        public string ComSpec { get; set; }
        public string DotNetFramework { get; set; }
        public string DotNetFramework_470 { get; set; }
        public string DotNetFramework_470_x64 { get; set; }
        public string HOMEDRIVE { get; set; }
        public string HOMEPATH { get; set; }
        public string InteractiveSession { get; set; }
        public string LOCALAPPDATA { get; set; }
        public string LOGONSERVER { get; set; }
        public string MSBuild { get; set; }
        public string MSBuild_40 { get; set; }
        public string MSBuild_40_x64 { get; set; }
        public string MSBuild_x64 { get; set; }
        public string NUMBER_OF_PROCESSORS { get; set; }
        public string OneDrive { get; set; }
        public string OS { get; set; }
        public string Path { get; set; }
        public string PATHEXT { get; set; }
        public string PowerShell { get; set; }
        public string PROCESSOR_ARCHITECTURE { get; set; }
        public string PROCESSOR_IDENTIFIER { get; set; }
        public string PROCESSOR_LEVEL { get; set; }
        public string PROCESSOR_REVISION { get; set; }
        public string ProgramData { get; set; }
        public string ProgramFiles { get; set; }
        public string ProgramFilesx86 { get; set; }
        public string ProgramW6432 { get; set; }
        public string PROMPT { get; set; }
        public string PSModulePath { get; set; }
        public string PUBLIC { get; set; }
        public string SESSIONNAME { get; set; }
        public string SLBSLS_LICENSE_FILE { get; set; }
        public string SystemDrive { get; set; }
        public string SystemRoot { get; set; }
        public string TEMP { get; set; }
        public string TMP { get; set; }
        public string UATDATA { get; set; }
        public string USERDNSDOMAIN { get; set; }
        public string USERDOMAIN { get; set; }
        public string USERDOMAIN_ROAMINGPROFILE { get; set; }
        public string USERNAME { get; set; }
        public string USERPROFILE { get; set; }
        public string VERBOSE_ARG { get; set; }
        public string VS120COMNTOOLS { get; set; }
        public string windir { get; set; }
        public string DotNetFramework_20 { get; set; }
        public string DotNetFramework_20_x64 { get; set; }
        public string DotNetFramework_30 { get; set; }
        public string DotNetFramework_30_x64 { get; set; }
        public string DotNetFramework_35 { get; set; }
        public string DotNetFramework_35_x64 { get; set; }
        public string DotNetFramework_462 { get; set; }
        public string DotNetFramework_462_x64 { get; set; }
        public string FP_NO_HOST_CHECK { get; set; }
        public string java { get; set; }
        public string java_7_x64 { get; set; }
        public string MSBuild_120 { get; set; }
        public string MSBuild_120_x64 { get; set; }
        public string MSBuild_20 { get; set; }
        public string MSBuild_20_x64 { get; set; }
        public string MSBuild_35 { get; set; }
        public string MSBuild_35_x64 { get; set; }
        public string SqlPackage { get; set; }
        public string VisualStudio { get; set; }
        public string VisualStudio_120 { get; set; }
        public string VisualStudio_IDE { get; set; }
        public string VisualStudio_IDE_120 { get; set; }
        public string VS110COMNTOOLS { get; set; }
        public string VSTest { get; set; }
        public string VSTest_120 { get; set; }
        public string windows_tracing_flags { get; set; }
        public string windows_tracing_logfile { get; set; }
        public string WindowsKit { get; set; }
        public string WindowsKit_80 { get; set; }
        public string WindowsKit_81 { get; set; }
        public string WindowsSdk { get; set; }
        public string WindowsSdk_71 { get; set; }
        public string WindowsSdk_80 { get; set; }
        public string WindowsSdk_80_NetFx35Tools { get; set; }
        public string WindowsSdk_80_NetFx35Tools_x64 { get; set; }
        public string WindowsSdk_80_NetFx40Tools { get; set; }
        public string WindowsSdk_80_NetFx40Tools_x64 { get; set; }
        public string WindowsSdk_81 { get; set; }
        public string WindowsSdk_81_NetFx40Tools { get; set; }
        public string WindowsSdk_81_NetFx40Tools_x64 { get; set; }
        public string CLIENTNAME { get; set; }
        public string DotNetFramework_461 { get; set; }
        public string DotNetFramework_461_x64 { get; set; }
        public string java_8 { get; set; }
        public string VisualStudio_100 { get; set; }
        public string VisualStudio_IDE_100 { get; set; }
        public string nodejs { get; set; }
        public string npm { get; set; }
        public string SCVMMAdminConsole { get; set; }
        public string MSBuild_140 { get; set; }
        public string MSBuild_140_x64 { get; set; }
        public string DotNetFramework_451 { get; set; }
        public string DotNetFramework_451_x64 { get; set; }
        public string MpConfig_ProductAppDataPath { get; set; }
        public string MpConfig_ProductCodeName { get; set; }
        public string MpConfig_ProductPath { get; set; }
        public string MpConfig_ProductUserAppDataPath { get; set; }
        public string MpConfig_ReportingGUID { get; set; }
        public string _ { get; set; }
        public string __CF_USER_TEXT_ENCODING { get; set; }
        public string ANDROID_HOME { get; set; }
        public string AndroidSDK { get; set; }
        public string Apple_PubSub_Socket_Render { get; set; }
        public string clang { get; set; }
        public string CLASSPATH { get; set; }
        public string curl { get; set; }
        public string git { get; set; }
        public string HOME { get; set; }
        public string JAVA_HOME { get; set; }
        public string JDK { get; set; }
        public string LC_CTYPE { get; set; }
        public string LOGNAME { get; set; }
        public string make { get; set; }
        public string PATH { get; set; }
        public string PWD { get; set; }
        public string python { get; set; }
        public string rake { get; set; }
        public string ruby { get; set; }
        public string SECURITYSESSIONID { get; set; }
        public string sh { get; set; }
        public string SHELL { get; set; }
        public string SSH_AUTH_SOCK { get; set; }
        public string subversion { get; set; }
        public string svn { get; set; }
        public string TERM_SESSION_ID { get; set; }
        public string TMPDIR { get; set; }
        public string USER { get; set; }
        public string VISUAL_HOME { get; set; }
        public string XamarinAndroid { get; set; }
        public string XamariniOS { get; set; }
        public string xcode { get; set; }
        public string XPC_FLAGS { get; set; }
        public string XPC_SERVICE_NAME { get; set; }
        public string TestBuildName { get; set; }
        public string COMPOSE_API_VERSION { get; set; }
        public string docker { get; set; }
        public string DOCKER_API_VERSION { get; set; }
        public string DOCKER_TOOLBOX_INSTALL_PATH { get; set; }
        public string java_8_x64 { get; set; }
        public string VBOX_MSI_INSTALL_PATH { get; set; }
        public string MSMPI_BIN { get; set; }
        public string AQtime8Install { get; set; }
        public string VS140COMNTOOLS { get; set; }
    }

    public class Usercapabilities
    {
        public string AutoGroup { get; set; }

        public string TestSet { get; set; }
        public string DotNetFramework { get; set; }
    }

   

    public class Web
    {
        public string href { get; set; }
    }

    public class Authorization
    {
        public string clientId { get; set; }
        public Publickey publicKey { get; set; }
    }

    public class Publickey
    {
        public string exponent { get; set; }
        public string modulus { get; set; }
    }


}

<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Azure.EmailNotification" generation="1" functional="0" release="0" Id="ab88488d-46c7-4355-b4f1-bba203352c56" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Azure.EmailNotificationGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="WorkerRole.EmailNotification:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/MapWorkerRole.EmailNotification:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole.EmailNotificationInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/MapWorkerRole.EmailNotificationInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapWorkerRole.EmailNotification:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/WorkerRole.EmailNotification/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole.EmailNotificationInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/WorkerRole.EmailNotificationInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WorkerRole.EmailNotification" generation="1" functional="0" release="0" software="C:\SourceCodes\DFrontierAppWizard\Azure.EmailNotification\csx\Release\roles\WorkerRole.EmailNotification" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="768" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole.EmailNotification&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRole.EmailNotification&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/WorkerRole.EmailNotificationInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/WorkerRole.EmailNotificationUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Azure.EmailNotification/Azure.EmailNotificationGroup/WorkerRole.EmailNotificationFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WorkerRole.EmailNotificationUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WorkerRole.EmailNotificationFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WorkerRole.EmailNotificationInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>
# Sitecore file browser

Very nice.  What’s it for?

<em>Browsing the file system??? Haha</em>

<em>Yeah good point. Let's you see and download files from your authoring and delivery servers, and any other sitecore box. A bit like a poor man's version of the [azure kudu tool](https://github.com/projectkudu/kudu).</em>

<em>In the cloud sometimes getting access is a pain. 
Primarily to allow the devs to collect logs as they can't be arsed centralising the logging somewhere.</em>

Sure and if there’s any security issues we can blame that on you too :joy:
Why don’t you use the admin logs page to get the logs? Or is it hardened so no admin pages enabled?

<em>It's secured via microchap, from [Kam Figy](https://github.com/kamsar/MicroCHAP)</em>

<em>Yes, your delivery servers are hardened. Aren't they.</em>

<em>I was planning on plugging in the [Monoca editor](https://github.com/Microsoft/monaco-editor) as well, might allow doing a diff between files across servers.</em>

1. Build project

2. Copy the following files:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -Sitecorefilebrowser.dll to your sitecore bin folder<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -MicroCHAP.dll to your sitecore bin folder (or use Unicorns)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -the App_Config folder<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; -the sitecore/admin/Sitefilebrowser/default.aspx<br/>

3. Set the shared secret in SitecoreFileBrowser.config

4. Deploy to any other remote machines

5. Navigate to /sitecore/admin/Sitecorefilebrowser


![alt text](https://github.com/benmcevoy/SitecoreFileBrowser/blob/master/readme.PNG "A screenshot")

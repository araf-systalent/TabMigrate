using OnlineContentDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

class MultiTaskMaster
    {
    public List<TaskMaster> importTaskMasterList;
    TaskMaster exportTaskMaster;
    public bool isDone;
    public string exportTargetPath;
    public string exportTargetPathBase;
    public bool isSiteAdmin;
    public string exportOnlySingleProject ;
    public string exportOnlyWithTag;
    public string importSitesCredentialsJSONPath;
    public bool remapContentOwnership;
    public FormSiteExportImport formSiteExportImport;
    List<String> importSiteNames;
    public List<SitesCredentials> sitesCredentials;
    System.Timers.Timer checkTimer;
    System.Timers.Timer checkImportsTimer;
    public MultiTaskMaster(TaskMaster taskMaster)
    {
        importTaskMasterList = new List<TaskMaster>();
        exportTaskMaster = taskMaster;
        isDone = false;
    }    

    public void startExicution()
    {
        if  (exportTaskMaster==null || exportTaskMaster.IsDone)
        {
            //isDone = true;
        }
        else
        {
            exportTaskMaster.ExecuteTaskBegin();
            isDone = false;
            checkTimer = new System.Timers.Timer(2000);
            checkTimer.Elapsed += checkExportIsCompleted;
            checkTimer.AutoReset = true;
            checkTimer.Enabled = true;
        }
    }
    private void checkExportIsCompleted(Object source, ElapsedEventArgs e)
    {
        if(exportTaskMaster==null || exportTaskMaster.IsDone == true)
        {
            checkTimer.Stop();
            checkTimer.Dispose();
            if(exportTaskMaster!=null && exportTaskMaster.IsDone == true)
            {
                exectueImportTasks();
            }
                
        }

    }
   

    public void getMultTaskFullStatus(out string status,out string errorStatus )
    {
        //string status = null;
        status = "";
        errorStatus = "";
        if (exportTaskMaster != null)
        {
            status = exportTaskMaster.StatusLog.StatusText;
            errorStatus= exportTaskMaster.StatusLog.ErrorText;
            int errorCount = exportTaskMaster.StatusLog.ErrorCount;
            string errorCountText = "";
            if (errorCount > 0)
            {
                errorCountText = " " + errorCount.ToString() + " errors";
            }

            status = status + "\r\n\r\n" + DateTime.Now.ToString() + " Done." + errorCountText + "\r\n";
            if (importTaskMasterList != null)
            {
                foreach (TaskMaster item in importTaskMasterList)
                {
                    status = status + "\n\n" + item.StatusLog.StatusText;
                     errorCount = item.StatusLog.ErrorCount;
                     errorCountText ="";
                    if (errorCount > 0)
                    {
                        errorCountText = " " + errorCount.ToString() + " errors";
                    }
                    if (isDone)
                    {
                        status = status + "\r\n\r\n" + DateTime.Now.ToString() + " Done." + errorCountText + "\r\n";
                    }
                    else
                    {
                        status = status + "\r\n\r\n" + DateTime.Now.ToString() + " Running." + errorCountText + "\r\n";
                    }
                    
                    errorStatus = errorStatus+"\n\n"+ item.StatusLog.ErrorText;
                }
            }

        }
        
    }


    private void checkImportsCompleted(Object source, ElapsedEventArgs e)
    {
        bool done = true;
        foreach (TaskMaster item in importTaskMasterList)
        {
            if (item.IsDone == false)
            {
                done = false;
            }
        }
        if (done != false)
        {
            isDone = true;
            checkImportsTimer.Stop();
            checkImportsTimer.Dispose();
            MessageBox.Show("Exports Completed at:" + DateTime.Now.ToString(), "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            isDone = false;
        }
    }
    private void exectueImportTasks()
    {
        formSiteExportImport.createImportTasks();
        foreach (TaskMaster item in importTaskMasterList)
        {
            item.ExecuteTaskBegin();
        }
        checkImportsTimer = new System.Timers.Timer(2000);
        checkImportsTimer.Elapsed += checkImportsCompleted;
        checkImportsTimer.AutoReset = true;
        checkImportsTimer.Enabled = true;

        /*var exportTargetPathBase = formSiteExportImport.GeneratePathFromSiteUrl(exportTaskMaster.TableauServerUrls);
        int i = 1;
        foreach (SitesCredentials item in sitesCredentials)
        {
            i++;
            var date = DateTime.Now;
            string localPathToImport = FileIOHelper.PathDateTimeSubdirectory(exportTargetPathBase, true, "si" + i, date);
            FileIOHelper.makeCopyToTarget(exportTargetPath, localPathToImport);
            TaskMaster importTaskMAster;
            formSiteExportImport.createAsychImportTask(item, isSiteAdmin, localPathToImport, remapContentOwnership, false,out importTaskMAster);
            addoImportTaskMasterList(importTaskMAster);
        }
        foreach (TaskMaster item in importTaskMasterList)
        {
            item.ExecuteTaskBegin();
        }*/
    }

    public void addoImportTaskMasterList(TaskMaster taskMaster)
    {
        importTaskMasterList.Add(taskMaster);
    }



    }


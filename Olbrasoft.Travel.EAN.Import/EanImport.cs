using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
#pragma warning disable 618
using static Castle.MicroKernel.Registration.AllTypes;
#pragma warning restore 618
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DAL.EntityFramework;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    class EanImport
    {
        public static ILogger Logger = new ConsoleLogger();
        //public static int UserId;

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var user = new User
            {
                UserName = "EanImport"
            };

            var container = BuildContainer();
            WriteContent(container);

            var usersFacade = container.Resolve<IUsersFacade>();
            usersFacade.AddIfNotExist(ref user);

            Write($"Id to a user with a UserName {user.UserName} is {user.Id}.");

            var startStatus = new Status
            {
                Name = "Start",
                Creator = user
            };

            var statusesFacade = container.Resolve<IStatusesFacade>();
            statusesFacade.AddIfNotExist(ref startStatus);

            Write($"Id to a Status with a Name {startStatus.Name} is {startStatus.Id}.");

            var import = new Travel.DTO.Import
            {
                Description = "Import ParentRegionList",
                Creator = user,
                CurrentStatus = startStatus
            };

            var importsFacade = container.Resolve<IImportsFacade>();
            importsFacade.Add(import);

            Write($"Id to a Import with a Description {import.Description} is {import.Id}.");

            var runningStatus = new Status
            {
                Name = "Running",
                Creator = user
            };

            statusesFacade.AddIfNotExist(ref runningStatus);

            Write($"Id to a Status with a Name {runningStatus.Name} is {runningStatus.Id}.");

            importsFacade.Update(import);
            
            var url = "https://www.ian.com/affiliatecenter/include/V2/ParentRegionList.zip";

           // DownloadFile(url, runningStatus, importsFacade, import);
            




            var sucessStatus = new Status
            {
                Name = "Sucess",
                Creator = user
            };

            statusesFacade.AddIfNotExist(ref sucessStatus);
            Write($"Id to a Status with a Name {sucessStatus.Name} is {sucessStatus.Id}.");
            

            Write("Imported");
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static async void DownloadFile(string url, Status runningStatus,
            IImportsFacade importsFacade, Travel.DTO.Import import)
        {
            var downloadParentRegionListTaskOfImport = new TaskOfImport
            {
                Description = "Download ParentRegionList"
            };

            import.Tasks.Add(downloadParentRegionListTaskOfImport);
            import.CurrentStatus = runningStatus;
            importsFacade.Update(import);

            var fileName = System.IO.Path.GetFileName(url);
            
            using (var wc = new WebClient())
            {
                wc.DownloadProgressChanged += (sender, eventArgs) =>
                {
                    if (downloadParentRegionListTaskOfImport.Progress == eventArgs.ProgressPercentage) return;
                    downloadParentRegionListTaskOfImport.Progress = eventArgs.ProgressPercentage;
                    importsFacade.Update(import);
                };

                await wc.DownloadFileTaskAsync(new Uri(url), @"D:\Ean\" + fileName);
            }
        }

        
        private static void WriteContent(IWindsorContainer container)
        {
#if DEBUG
            foreach (var handler in container.Kernel
                .GetAssignableHandlers(typeof(object)))
            {
                Write($"{handler.ComponentModel.Services} {handler.ComponentModel.Implementation}");
            }
#endif
        }


        private static WindsorContainer BuildContainer()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<TravelContext>
                ().ImplementedBy<TravelContext>());

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.DAL.EntityFramework")
                .Where(type => type.Name.EndsWith("Repository"))
                .WithService.AllInterfaces()
            );

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.BLL")
                .Where(type => type.Name.EndsWith("Facade"))
                .WithService.AllInterfaces()
            );


            return container;
        }





        public static void Write(object s)
        {
#if DEBUG
            Logger.Log(s.ToString());
#endif
        }



       
    }
}

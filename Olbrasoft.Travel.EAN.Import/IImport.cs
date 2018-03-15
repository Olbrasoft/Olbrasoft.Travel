namespace Olbrasoft.Travel.EAN.Import
{
   public interface IImport<in T>
    {
        void Import(string path);
       
    }
}

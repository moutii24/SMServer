using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MouseClientRequester;
using MySql.Data;
using MySql.Data.MySqlClient;
using SqlConnectionDialog;
/// <summary>
/// Description résumée de MouseSubmit
/// </summary>
[WebService(Namespace = "SmartMouseServer.SubmitMouse")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
// [System.Web.Script.Services.ScriptService]
public class MouseSubmit : System.Web.Services.WebService {

    public MouseSubmit () {

        //Supprimez les marques de commentaire dans la ligne suivante si vous utilisez des composants conçus 
        //InitializeComponent(); 
    }

   [WebMethod]
    public Boolean DumpData()
    {

       
       
       return true;
    }     


    public void RecieveConnection(){
        MouseClientRequester.mouseInformations mouseInfo = new MouseClientRequester.mouseInformations();
        MouseClientRequester.RequesterClient ws = new RequesterClient();

        mouseInfo = ws.SubmitMouse();
      
        string MyConnection = "Server=127.0.0.1;Port=3306;Database=mouses_liste;Uid=root;Pwd=admin";
        //string Query = "insert into mouses_liste.connectedmouses(mouseID,userIP,connectionDate) values('" + mouseInfo.IDmouse + "','" + mouseInfo.IPuser + "','" + mouseInfo.dateConnection + "');";
        string Query = "insert into mouses_liste.connectedmouses(mouseID,userIP,connectionDate) values(@mouseID,@userIP,@connectionDate);";

        MySqlConnection MyConn = new MySqlConnection(MyConnection);
        MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
        MySqlDataReader MyReader;

        MyCommand.Parameters.AddWithValue("@mouseID", mouseInfo.IDmouse);
        MyCommand.Parameters.AddWithValue("@userIP", mouseInfo.IPuser);
        MyCommand.Parameters.AddWithValue("@connectionDate", mouseInfo.dateConnection);

        MyConn.Open();
        MyReader = MyCommand.ExecuteReader();       
        MyConn.Close(); 
        //return true;
    }

    public void SaveDataDB()
    {
        Dumper.mesures[] mesure;
        Dumper.DumpMesrusesClient dump = new Dumper.DumpMesrusesClient();

        mesure = dump.DumpDataDB();
        string MyConnection = "Server=127.0.0.1;Port=3306;Database=collected_data_mouses;Uid=root;Pwd=admin";
        //string Query = "insert into mouses_liste.connectedmouses(mouseID,userIP,connectionDate) values('" + mouseInfo.IDmouse + "','" + mouseInfo.IPuser + "','" + mouseInfo.dateConnection + "');";
        string Query = "insert into collected_data_mouses.mouse_values(condValue,tempValue,mesureDate,mouseID) values(@condValue,@tempValue,@mesureDate,@mouseID);";

        MySqlConnection MyConn = new MySqlConnection(MyConnection);
        MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
        MySqlDataReader MyReader;

        for (int i = 0; i < mesure.Length; i++) { 

            MyCommand.Parameters.AddWithValue("@condValue", mesure[i].condValue);
            MyCommand.Parameters.AddWithValue("@tempValue", mesure[i].tempValue);
            MyCommand.Parameters.AddWithValue("@mesureDate", mesure[i].mesureDate);
            MyCommand.Parameters.AddWithValue("@mesureID", mesure[i].getM);
            
            MyConn.Open();
            MyReader = MyCommand.ExecuteReader();     
            MyConn.Close(); 
        }
    }

    [WebMethod]
    public Boolean SubmitMouse() {
        RecieveConnection();     
        return true;
    }
    

}
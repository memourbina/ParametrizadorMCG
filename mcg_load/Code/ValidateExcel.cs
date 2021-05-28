using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Schema;
using ExcelDataReader;
using mcg_load.Models.FileUpload;
using mcg_load.Models.upload;


namespace mcg_load.Code
{
    public class ValidateExcel
    {
        #region Variables y Constantes

        private String strParseError = "";
        //string UploadDirectory = "C:/Users/Lenovo/source/repos/mcg/mcg_load/Content/UploadControl/UploadFolder
        private string UploadDirectory = WebConfigurationManager.AppSettings["UploadDirectory"]; //"C:/FilesExcel/UploadFolder/";
        string UploadDirectoryXml = WebConfigurationManager.AppSettings["UploadDirectoryXml"]; //"C:/FilesExcel/xml/";
        private XmlSchemaSet schemas;
        XmlReaderSettings settings;
        static bool valid = true;
        private string fileName;
        //private string safeFileName;
        private mcg_saturacionEntities db = new mcg_saturacionEntities();
        private Esc_File_Upload EscFileUpload;
        private Esc_Escenario escEscenario;


        private readonly String SQL_UPDATE_CAVIDADES =
            "UPDATE A SET Cantidad_base = Cantidad FROM PVO_Escenarios.dbo.Esc_Cavidades A WHERE id_escenario = @id_escenario;";

        private readonly String SQL_UPDATE_CATALOGO_ORACLE = "UPDATE A SET " +
                                                             "[Org# ID] = P1.org_id " +
                                                             ",[Línea Moldeo]= SUBSTRING(I.[Linea],1,30), [Cliente]=SUBSTRING(I.[OEM],1,30) " +
                                                             ",[Customer Name]=SUBSTRING(C.[RazonSocial],1,120), [Industria] = SUBSTRING(I.[TipoIndustria],1,30) " +
                                                             ",[Tipo Pieza]=SUBSTRING(I.[TipoPieza],1,30) ,[Description]=SUBSTRING(CI.[Pro_Descripcion],1,120) " +
                                                             ",[No# Oracle]=SUBSTRING(CI.[Pro_ItemAntiguo],1,60) ,[Unit Weight]=CI.[Pro_PesoA4] " +
                                                             ",[Tipo de Hierro]=SUBSTRING(I.[TipoHierro],1,20) ,[Grado de Hierro] = SUBSTRING(I.[Metal],1,20) " +
                                                             ",[OEM]=SUBSTRING(I.[OEM],1,30) ,[Plataforma]=SUBSTRING(I.[PlataformaComercial],1,30) " +
                                                             ",[Sistema]=SUBSTRING(I.[Sistema],1,20) ,[Planta]=SUBSTRING(P.[Nombre],1,20) " +
                                                             "FROM [PVO_Escenarios].[dbo].[Esc_Articulos_staging] A " +
                                                             "LEFT JOIN[PVO].[dbo].[CAT_Items] CI ON A.[No# Oracle] = CI.[Pro_ItemAntiguo] COLLATE DATABASE_DEFAULT " +
                                                             "INNER JOIN PVO.dbo.CAT_ItemsParametros I ON   CI.[Pro_ItemId] = I.[ItemID] AND  CI.[Pro_OrganizacionInventario] = I.[OI] " +
                                                             "INNER JOIN[PVO].[dbo].[CAT_ItemsCliente]  C ON   I.[ItemID] = C.[ItemID] AND  I.[OI] = C.[OI] " +
                                                             "INNER JOIN[PVO].[dbo].[CAT_Plantas] P ON SUBSTRING(P.[OI], 1, 3) = I.[OI] COLLATE DATABASE_DEFAULT " +
                                                             "INNER JOIN PVO_Escenarios.dbo.CAT_Plantas P1 ON  P1.Planta = P.OI " +
                                                             "WHERE A.id_escenario = @id_escenario ";

        private readonly String SQL_INSERT_CATALOGO = "INSERT INTO [PVO_Escenarios].[dbo].[Esc_Articulos] ([id_articulo],[id_escenario],[Org# ID],[Escenario],[Probabilidad]" +
                                                      ",[Probabilidad_base],[Línea Moldeo],[Cliente],[Customer Name],[Industria],[Tipo Pieza],[Description],[No# Oracle]" +
                                                      ",[Customer PN],[Unit Weight],[Mercado],[Tipo de Hierro],[Grado de Hierro],[OEM],[Plataforma],[Sistema],[Cavs /  Soplo]" +
                                                      ",[Soplos / Hora],[Cor / pza],[Maquina],[id_maquina],[Planta],[Planta01],[Linea],[modificado],[UserId_modifico]" +
                                                      ",[fecha_modificacion])" +
                                                      "(SELECT [id_articulo],[id_escenario],[Org# ID],[Escenario],[Probabilidad],[Probabilidad_base],[Línea Moldeo],[Cliente]" +
                                                      ",[Customer Name],[Industria],[Tipo Pieza],[Description],[No# Oracle],[Customer PN],[Unit Weight],[Mercado],[Tipo de Hierro]" +
                                                      ",[Grado de Hierro],[OEM],[Plataforma],[Sistema],[Cavs /  Soplo],[Soplos / Hora],[Cor / pza],[Maquina],[id_maquina],[Planta]" +
                                                      ",[Planta01],[Linea],[modificado],[UserId_modifico],[fecha_modificacion] " +
                                                      "FROM [PVO_Escenarios].[dbo].[Esc_Articulos_staging] " +
                                                      "WHERE[id_escenario] = @id_escenario )";

        private readonly String SQL_UPDATE_VELOCIDAD =
            "UPDATE A SET Valor_base = Valor FROM PVO_Escenarios.dbo.Esc_Velocidad A WHERE id_escenario = @id_escenario;";

        private readonly String SQL_UPDATE_RECHAZOS =
            "UPDATE A SET Interno_base=Interno, Externo_base=Externo, Combinado_base=Combinado FROM PVO_Escenarios.dbo.Esc_Rechazo A WHERE id_escenario = @id_escenario;";

        private readonly string SQL_INSERT_DEMANDA = "INSERT INTO PVO_Escenarios.dbo.Esc_Demanda " +
                                                     "(id_articulo, Fecha, id_escenario, UserId, Valor, valor_base, eliminado) " +
                                                     "( SELECT id_articulo, CAST(FORMAT(Fecha,'yyyyMMdd') as int), id_escenario, UserId, Valor, Valor, eliminado " +
                                                     " FROM PVO_Escenarios.dbo.Esc_Demanda_staging WHERE id_escenario = @id_escenario);";

        private readonly string SQL_DELETE_DEMANDA =
            "DELETE FROM PVO_Escenarios.dbo.Esc_Demanda_staging WHERE id_escenario = @id_escenario;";


        private readonly string SQL_UPDATE_ARTICULO_PLANTA =
            "UPDATE A SET A.Planta01 = P.Planta FROM PVO_Escenarios.dbo.Esc_Articulos A INNER JOIN PVO_Escenarios.dbo.CAT_Plantas P ON A.[Org# ID] = P.org_id WHERE id_escenario = @id_escenario";

        private readonly string SQL_UPDATE_ARTICULO_LINEA =
            "UPDATE A SET A.Linea = L.Linea FROM PVO_Escenarios.dbo.Esc_Articulos A INNER JOIN PVO_Escenarios.dbo.Esc_Lineas L ON A.[Línea Moldeo] = L.Linea WHERE id_escenario = @id_escenario";

        private readonly string SQL_INSERT_HORAS =
            "INSERT INTO PVO_Escenarios.dbo.Esc_Horas (Planta, Linea, Fecha, id_escenario, UserId, Horas, Horas_base, modificado, UserId_modifico, fecha_modificacion) " +
            "(SELECT Planta, Linea, CAST(FORMAT(Fecha,'yyyyMMdd') as int), id_escenario, UserId, Horas, Horas_base, modificado, UserId_modifico, fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_Horas_staging WHERE id_escenario = @id_escenario );";

        private readonly string SQL_DELETE_HORAS =
            "DELETE FROM PVO_Escenarios.dbo.Esc_Horas_staging WHERE id_escenario = @id_escenario;";

        private readonly string SQL_INSERT_HORASCORAZONES =
            "INSERT INTO PVO_Escenarios.dbo.Esc_HorasCorazones (Maquina, Fecha, id_escenario, UserId, Horas, Horas_base, modificado, UserId_modifico, fecha_modificacion) " +
            "(SELECT Maquina, CAST(FORMAT(Fecha, 'yyyyMMdd') as int), id_escenario, UserId, Horas, Horas_base, modificado, UserId_modifico, fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_HorasCorazones_staging " +
            "WHERE id_escenario = @id_escenario);";

        private readonly string SQL_DELETE_HORASCORAZONES =
            "DELETE FROM PVO_Escenarios.dbo.Esc_HorasCorazones_staging WHERE id_escenario = @id_escenario;";

        private readonly string SQL_INSERT_OEE =
            "INSERT INTO PVO_Escenarios.dbo.Esc_OEE (Planta, Linea, Fecha, id_escenario, UserId, OEE_SinRechazo, OEE_SinRechazo_base, modificado, UserId_modifico, fecha_modificacion) " +
            " (SELECT L.Planta, O.Linea, CAST(FORMAT(O.Fecha, 'yyyyMMdd') as int), O.id_escenario, O.UserId, O.OEE_SinRechazo, O.OEE_SinRechazo_base, O.modificado, O.UserId_modifico, O.fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_OEE_staging O INNER JOIN PVO_Escenarios.dbo.Esc_Lineas L ON L.Linea = O.Linea WHERE id_escenario = @id_escenario);";

        private readonly string SQL_DELETE_OEE =
            "DELETE FROM PVO_Escenarios.dbo.Esc_OEE_staging WHERE id_escenario = @id_escenario;";


        private readonly string SQL_SHRINK_DATABASE =
            "ALTER DATABASE 'PVO_Escenarios' SET RECOVERY SIMPLE;DBCC SHRINKFILE (2, 1); GO ALTER DATABASE 'db' SET RECOVERY FULL;";

        private static string SQL_CAVIDADES_FROM_ORACLE =
            "  INSERT INTO PVO_Escenarios.dbo.Esc_Cavidades " + 
            "(id_articulo, Año, id_escenario, UserId, Cantidad, Cantidad_base, eliminado, modificado, UserId_modifico, fecha_modificacion) " +
            "(SELECT " +
            " A.id_articulo               as id_articulo " +
            ",AN.Año                      as Año " +
            ",A.id_escenario              as id_escenario " +
            ",'{0}'                       as UserId " +
            ",MAX(C.CavidadesHabilitadas) as Cantidad " +
            ",MAX(C.CavidadesHabilitadas) as Cantidad_base " +
            ",0                           as eliminado " +
            ",null                        as modificado " +
            ",null                        as UserId_modifico " +
            ",null                        as fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_Articulos A " +
            "LEFT JOIN PVO.dbo.CAT_Items CI ON A.[No# Oracle] = CI.Pro_ItemAntiguo COLLATE DATABASE_DEFAULT  AND A.Planta01 = CI.Pro_OrganizacionInventario " +
            "LEFT JOIN PVO.dbo.CAT_ItemsCavidades C ON C.ItemID = CI.Pro_ItemId AND C.OI = CI.Pro_OrganizacionInventario " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Año AN ON  AN.Año > {1} " +
            "LEFT JOIN PVO_Escenarios.dbo.Esc_Cavidades CA ON  CA.Año = AN.Año AND CA.id_escenario = A.id_escenario AND CA.id_articulo = A.id_articulo " +
            "WHERE A.id_escenario = @id_escenario " +
            "AND CA.id_articulo IS NULL " +
            "GROUP BY A.id_articulo, AN.Año, A.id_escenario);";

        private static string SQL_VELOCIDAD_FROM_ORACLE =
            "INSERT INTO PVO_Escenarios.dbo.Esc_Velocidad " +
            "(id_articulo, Año, id_escenario, UserId, Valor, Valor_base, eliminado, modificado, fecha_modificacion, UserId_modifico) " +
            "(SELECT " +
            " A.id_articulo          as id_articulo" +
            ",AN.Año                 as Año" +
            ",A.id_escenario         as id_escenario" +
            ",'{0}'                  as UserId" +
            ",MAX(C.MoldesXHora)     as Valor" +
            ",MAX(C.MoldesXHora)     as Valor_base" +
            ",0                      as eliminado" +
            ",null                   as modificado " +
            ",null                   as UserId_modifico " +
            ",null                   as fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_Articulos A " +
            "LEFT JOIN PVO.dbo.CAT_Items CI ON A.[No# Oracle] = CI.Pro_ItemAntiguo COLLATE DATABASE_DEFAULT AND A.Planta01 = CI.Pro_OrganizacionInventario " +
            "LEFT JOIN PVO.dbo.CAT_ItemsCavidades C ON C.ItemID = CI.Pro_ItemId AND C.OI = CI.Pro_OrganizacionInventario " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Año AN ON  AN.Año > {1} " +
            "LEFT JOIN PVO_Escenarios.dbo.Esc_Velocidad CA ON  CA.Año = AN.Año AND CA.id_escenario = A.id_escenario AND CA.id_articulo = A.id_articulo " +
            "WHERE A.id_escenario = @id_escenario AND CA.id_articulo IS NULL " +
            "GROUP BY A.id_articulo, AN.Año, A.id_escenario);";

        //private DataTable tempoDataTable;
        readonly string _Connection_String = null;
        private readonly string _userId = null;
        private string _nombre_tabla = null;
        private string _primer_error = null;
        private bool _error_repetido = false;

        public FileUploadResult UploadResult { get; private set; }

        private DataSet result;

        #endregion Variables y Constantes


        public ValidateExcel()
        {
            //string strBasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            //UploadDirectory = strBasePath  + "/Content/UploadControl/UploadFolder/";
            //UploadDirectoryXml = strBasePath + "/Content/UploadControl/xml/";

            _Connection_String = db.Database.Connection.ConnectionString;
        }

        public ValidateExcel(UploadExcelViewModel model, string userId)
        {
            UploadResult = new FileUploadResult();

            _Connection_String = db.Database.Connection.ConnectionString;
            Stream stream = model.FileAttach.InputStream;
            string strHash = "";
            _userId = userId;

            byte[] uploadedFile = new byte[model.FileAttach.InputStream.Length];
            model.FileAttach.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            fileName =
                Path.GetFileNameWithoutExtension(model.FileAttach.FileName) + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                Path.GetExtension(model.FileAttach.FileName);

            File.WriteAllBytes(UploadDirectory + fileName, uploadedFile);

            if (!FileUploadHelper.ValidateHashFile(UploadDirectory + fileName, stream, ref strHash, ref EscFileUpload))
            {
                UploadResult.isValid = false;
                UploadResult.MessageResult = "Archivo cargado previamente, carga de nombre -> " + EscFileUpload.nombre;
                UploadResult.FileUpload = EscFileUpload;
                BResultado = false;
                return;
            }

            // now you could pass the byte array to your model and store wherever 
            // you intended to store it
            //fileName =
            //    Path.GetFileNameWithoutExtension(model.FileAttach.FileName) + "_" +
            //    DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
            //    Path.GetExtension(model.FileAttach.FileName);

            //safeFileName =
            //    Path.GetFileNameWithoutExtension(model.FileAttach.FileName) + "_" +
            //    DateTime.Now.ToString("yyyyMMddHHmmss") + "_A." +
            //    Path.GetExtension(model.FileAttach.FileName);

            //System.IO.File.WriteAllBytes(UploadDirectory + fileName, uploadedFile);

            if (!this.CargarXml03(stream, model.FileAttach.FileName))
            {
                UploadResult.isValid = false;
                UploadResult.MessageResult = "Error en la Estructura !!!";
                UploadResult.FileUpload = EscFileUpload;
                return;
            }

            EscFileUpload = new Esc_File_Upload()
            {
                UserId = _userId,
                ContentLength = model.FileAttach.InputStream.Length,
                ContentType = Path.GetExtension(model.FileAttach.FileName),
                FileName = model.FileAttach.FileName,
                TempFileName = fileName,
                date_uplodad = DateTime.Now,
                eliminado = false,
                comentario = model.Message,
                nombre = model.Nombre,
                hashFile = strHash
            };

            db.Esc_File_Upload.Add(EscFileUpload);
            db.SaveChanges();

            escEscenario = new Esc_Escenario()
            {
                id_UpLoad = EscFileUpload.id_UpLoad,
                id_tipo_escenario = 1,
                UserId = _userId,
                comentario = model.Message,
                nombre = "Escenario Base Carga " + EscFileUpload.id_UpLoad,
                activo = true,
                fecha = DateTime.Now,
                eliminado = false,
                modificado = false
            };

            db.Esc_Escenario.Add(escEscenario);
            db.SaveChanges();


            CargarXml02();

            BResultado = true;

            UploadResult.isValid = true;
            UploadResult.MessageResult = "Archivo cargado !!!";
            UploadResult.FileUpload = EscFileUpload;
        }

        public bool BResultado { get; private set; }

        //public Esc_File_Upload EscFileUpload { get; private set; }

        //public void CargarXls()
        //{
        //    DataSet ds = this.ReadExcelFile(fileName);
        //    CreateXML(fileName.Replace("\\" + safeFileName, string.Empty), ds);
        //    this.ValidateExcelSchema(fileName.Replace("\\" + safeFileName, string.Empty));
        //    if (valid == true)
        //        this.ValidateExcelData(fileName.Replace("\\" + safeFileName, string.Empty));
        //    //this.validatexms("\\\\mum-exhgsrv\\Home\\ebsupport\\Desktop\\");
        //    //this.validateSchema(ds.GetXmlSchema());
        //}

        public bool CargarXml03(Stream stream, string fileName)
        {
            // We return the interface, so that
            IExcelDataReader reader = null;

            switch (Path.GetExtension(fileName))
            {
                case ".xls":
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
                case ".xlsx":
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    break;
                default:
                    Console.WriteLine("This file format is not supported");
                    return false;
            }
            //ModelState.AddModelError("File", "This file format is not supported");
            //return View();


            result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            reader.Close();
            bool bandera = true;
            foreach (DataTable resultTable in result.Tables)
            {
                _nombre_tabla = resultTable.TableName;
                bandera = bandera != false && CargaDB(resultTable, true);
            }

            return bandera;
        }

        public DataTable CargarXml02()
        {
            foreach (DataTable resultTable in result.Tables)
            {
                //Escribe esquemas individuales por hoja de archivo excel.
                //resultTable.WriteXmlSchema(UploadDirectory + resultTable.TableName + ".xsd");

                //Escribe esquema general por todo el libro del archivo excel.
                //result.WriteXmlSchema(UploadDirectory + resultTable.TableName + ".xsd");

                CargaDB(resultTable, false);
            }
            string strQuery = String.Format(SQL_CAVIDADES_FROM_ORACLE, _userId, 2009);

            db.Database.ExecuteSqlCommand(strQuery, new SqlParameter("@id_escenario", escEscenario.id_escenario));

            strQuery = String.Format(SQL_VELOCIDAD_FROM_ORACLE, _userId, 2009);

            db.Database.ExecuteSqlCommand(strQuery, new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return result.Tables[0];
        }

        private bool CargaDB(DataTable dataTable, bool bValidating)
        {
            bool bandera = true;
            switch (dataTable.TableName.ToLower())
            {
                case "catalogo":
                    if (bValidating) { bandera = ValidateXML(UploadDirectoryXml + "Catalogo.xsd", dataTable).Length == 0; }
                    else { bandera = db_catalogo(dataTable); }
                    break;
                case "cavidades":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "Cavidades.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_cavidades(dataTable);
                    }
                    break;
                case "demandap":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "DemandaP.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_demanda(dataTable);
                    }
                    break;
                case "horas":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "Horas.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_horas(dataTable);
                    }
                    break;
                case "horascorazones":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "HorasCorazones.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_horascorazones(dataTable);
                    }
                    break;
                case "oee":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "OEE.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_oee(dataTable);
                    }
                    break;
                case "oee_corazones":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "OEE_Corazones.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_oee_corazones(dataTable);
                    }
                    break;
                case "oee_desglose":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "OEE_Desglose.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_oee_desglose(dataTable);
                    }
                    break;
                case "parametros":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "Parametros.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_parametros(dataTable);
                    }
                    break;
                case "rechazo":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "Rechazo.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_rechazo(dataTable);
                    }

                    break;
                case "velocidad":
                    if (bValidating)
                    {
                        bandera = ValidateXML(UploadDirectoryXml + "Velocidad.xsd", dataTable).Length == 0;
                    }
                    else
                    {
                        bandera = db_velocidad(dataTable);
                    }

                    break;
                case "lineas": break;
                case "oee (2)": break;
            }

            return bandera;
        }

        //---- Carga Tabla 001
        private bool db_catalogo(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Articulos_staging";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("Planta", "Planta");
                    copy.ColumnMappings.Add("Org. ID", "Org# ID");
                    copy.ColumnMappings.Add("Escenario", "Escenario");
                    copy.ColumnMappings.Add("Probabilidad", "Probabilidad");
                    copy.ColumnMappings.Add("Probabilidad", "Probabilidad_base");
                    copy.ColumnMappings.Add("Línea Moldeo", "Línea Moldeo");
                    copy.ColumnMappings.Add("Cliente", "Cliente");
                    copy.ColumnMappings.Add("Customer Name", "Customer Name");
                    copy.ColumnMappings.Add("Industria", "Industria");
                    copy.ColumnMappings.Add("Tipo Pieza", "Tipo Pieza");
                    copy.ColumnMappings.Add("Description", "Description");
                    copy.ColumnMappings.Add("No. Oracle", "No# Oracle");
                    copy.ColumnMappings.Add("Customer PN", "Customer PN");
                    copy.ColumnMappings.Add("Unit Weight", "Unit Weight");
                    copy.ColumnMappings.Add("Mercado", "Mercado");
                    copy.ColumnMappings.Add("Tipo de Hierro", "Tipo de Hierro");
                    copy.ColumnMappings.Add("Grado de Hierro", "Grado de Hierro");
                    copy.ColumnMappings.Add("OEM", "OEM");
                    copy.ColumnMappings.Add("Plataforma", "Plataforma");
                    copy.ColumnMappings.Add("Sistema", "Sistema");
                    copy.ColumnMappings.Add("Cavs /  Soplo", "Cavs /  Soplo");
                    copy.ColumnMappings.Add("Soplos / Hora", "Soplos / Hora");
                    copy.ColumnMappings.Add("Cor / pza", "Cor / pza");
                    copy.ColumnMappings.Add("Maquina", "Maquina");

                    copy.WriteToServer(dataTable);
                }
                destinationConnection.Close();
            }
            db.Database.CommandTimeout = 500;

            db.Database.ExecuteSqlCommand(SQL_UPDATE_CATALOGO_ORACLE,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));

            db.Database.ExecuteSqlCommand(SQL_INSERT_CATALOGO,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));

            db.Database.ExecuteSqlCommand(SQL_UPDATE_ARTICULO_PLANTA,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            db.Database.ExecuteSqlCommand(SQL_UPDATE_ARTICULO_LINEA,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }

        //---- Carga Tabla 002
        private bool db_cavidades(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("Cantidad_base", typeof(System.Double)) { DefaultValue = 0.0 });
            dataTable.Columns.Add(new DataColumn("eliminado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                db.Database.CommandTimeout = 500;
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Cavidades";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("Cantidad", "Cantidad");

                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Cantidad_base", "Cantidad_base");
                    copy.ColumnMappings.Add("eliminado", "eliminado");

                    copy.WriteToServer(dataTable);
                }
                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_UPDATE_CAVIDADES,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));

            return true;
        }

        //---- Carga Tabla 003
        private bool db_demanda(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("eliminado", typeof(System.Boolean)) { DefaultValue = false });

            db.Database.CommandTimeout = 500;

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString) { BatchSize = 100, BulkCopyTimeout = 60 })
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Demanda_staging";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("Fecha", "Fecha");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Valor", "Valor");
                    copy.ColumnMappings.Add("Valor", "Valor_base");
                    copy.ColumnMappings.Add("eliminado", "eliminado");
                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_INSERT_DEMANDA,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            db.Database.ExecuteSqlCommand(SQL_DELETE_DEMANDA,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));

            return true;
        }

        //---- Carga Tabla 004
        private bool db_horas(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("modificado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Horas_staging";

                    copy.ColumnMappings.Add("Planta", "Planta");
                    copy.ColumnMappings.Add("Linea", "Linea");
                    copy.ColumnMappings.Add("Fecha", "Fecha");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Horas", "Horas");
                    copy.ColumnMappings.Add("Horas", "Horas_base");
                    copy.ColumnMappings.Add("modificado", "modificado");
                    //copy.ColumnMappings.Add("ID", "UserId_modifico");
                    //copy.ColumnMappings.Add("ID", "fecha_modificacion");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_INSERT_HORAS,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            db.Database.ExecuteSqlCommand(SQL_DELETE_HORAS,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));

            return true;
        }

        //---- Carga Tabla 011
        private bool db_horascorazones(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("eliminado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_HorasCorazones_staging";

                    copy.ColumnMappings.Add("Maquina", "Maquina");
                    copy.ColumnMappings.Add("Fecha", "Fecha");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Horas", "Horas");
                    copy.ColumnMappings.Add("Horas", "Horas_base");
                    copy.ColumnMappings.Add("eliminado", "modificado");
                    //copy.ColumnMappings.Add("ID", "UserId_modifico");
                    //copy.ColumnMappings.Add("ID", "fecha_modificacion");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_INSERT_HORASCORAZONES,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            db.Database.ExecuteSqlCommand(SQL_DELETE_HORASCORAZONES,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }

        //---- Carga Tabla 007
        private bool db_oee(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("modificado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_OEE_staging";

                    copy.ColumnMappings.Add("Planta", "Planta");
                    copy.ColumnMappings.Add("Linea", "Linea");
                    copy.ColumnMappings.Add("Fecha", "Fecha");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("OEE_SinRechazo", "OEE_SinRechazo");
                    copy.ColumnMappings.Add("OEE_SinRechazo", "OEE_SinRechazo_base");
                    copy.ColumnMappings.Add("modificado", "modificado");
                    //copy.ColumnMappings.Add("Año", "UserId_modifico");
                    //copy.ColumnMappings.Add("Año", "fecha_modificacion");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_INSERT_OEE, new SqlParameter("@id_escenario", escEscenario.id_escenario));
            db.Database.ExecuteSqlCommand(SQL_DELETE_OEE, new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }

        //---- Carga Tabla 009
        private bool db_oee_corazones(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("modificado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_OEECorazones";

                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Rendimiento", "Rendimiento");
                    copy.ColumnMappings.Add("Rendimiento", "Rendimiento_base");
                    copy.ColumnMappings.Add("Disponibilidad", "Disponibilidad");
                    copy.ColumnMappings.Add("Disponibilidad", "Disponibilidad_base");
                    copy.ColumnMappings.Add("Rechazo", "Rechazo");
                    copy.ColumnMappings.Add("Rechazo", "Rechazo_base");
                    copy.ColumnMappings.Add("OEE sin rechazo", "OEE sin rechazo");
                    copy.ColumnMappings.Add("OEE sin rechazo", "OEE sin rechazo_base");
                    copy.ColumnMappings.Add("OEE", "OEE");
                    copy.ColumnMappings.Add("OEE", "OEE_base");
                    copy.ColumnMappings.Add("modificado", "modificado");
                    //copy.ColumnMappings.Add("ID", "UserId_modifico");
                    //copy.ColumnMappings.Add("ID", "fecha_modificacion");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            //db.Database.ExecuteSqlCommand(SQL_UPDATE_CAVIDADES,
            //    new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }

        //---- Carga Tabla 008
        private bool db_oee_desglose(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("modificado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_OEEDesglose";

                    copy.ColumnMappings.Add("Planta", "Planta");
                    copy.ColumnMappings.Add("Linea", "Linea");
                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Disponibilidad", "Disponibilidad");
                    copy.ColumnMappings.Add("Disponibilidad", "Disponibilidad_base");
                    copy.ColumnMappings.Add("Rechazo", "Rechazo");
                    copy.ColumnMappings.Add("Rechazo", "Rechazo_base");
                    copy.ColumnMappings.Add("Rendimiento", "Rendimiento");
                    copy.ColumnMappings.Add("Rendimiento", "Rendimiento_base");
                    copy.ColumnMappings.Add("OEE sin rechazo", "OEE sin rechazo");
                    copy.ColumnMappings.Add("OEE sin rechazo", "OEE sin rechazo_base");
                    copy.ColumnMappings.Add("OEE", "OEE");
                    copy.ColumnMappings.Add("OEE", "OEE_base");
                    copy.ColumnMappings.Add("modificado", "modificado");
                    //copy.ColumnMappings.Add("Año", "UserId_modifico");
                    //copy.ColumnMappings.Add("Año", "fecha_modificacion");


                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            return true;
        }

        //---- Carga Tabla 006
        private bool db_parametros(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("modificado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Parametros";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("Parametro", "Parametro");
                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Valor", "Valor");
                    copy.ColumnMappings.Add("Valor", "Valor_base");
                    copy.ColumnMappings.Add("modificado", "modificado");
                    //copy.ColumnMappings.Add("Planta", "UserId_modifico");
                    //copy.ColumnMappings.Add("Planta", "fecha_modificacion");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            return true;
        }

        //---- Carga Tabla 003
        private bool db_rechazo(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("Interno_base", typeof(System.Double)) { DefaultValue = 0.0 });
            dataTable.Columns.Add(new DataColumn("Externo_base", typeof(System.Double)) { DefaultValue = 0.0 });
            dataTable.Columns.Add(new DataColumn("Combinado_base", typeof(System.Double)) { DefaultValue = 0.0 });
            dataTable.Columns.Add(new DataColumn("eliminado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Rechazo";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("Interno", "Interno");
                    copy.ColumnMappings.Add("Externo", "Externo");
                    copy.ColumnMappings.Add("Combinado", "Combinado");

                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Interno_base", "Interno_base");
                    copy.ColumnMappings.Add("Externo_base", "Externo_base");
                    copy.ColumnMappings.Add("Combinado_base", "Combinado_base");
                    copy.ColumnMappings.Add("eliminado", "eliminado");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_UPDATE_RECHAZOS,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }

        //---- Carga Tabla 004
        private bool db_velocidad(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("id_escenario", typeof(System.Int64))
            { DefaultValue = escEscenario.id_escenario });
            dataTable.Columns.Add(new DataColumn("UserId", typeof(System.String)) { DefaultValue = _userId });
            dataTable.Columns.Add(new DataColumn("Valor_base", typeof(System.Double)) { DefaultValue = 0.0 });
            dataTable.Columns.Add(new DataColumn("eliminado", typeof(System.Boolean)) { DefaultValue = false });

            using (SqlConnection destinationConnection = new SqlConnection(_Connection_String))
            {
                destinationConnection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(destinationConnection.ConnectionString))
                {
                    copy.DestinationTableName = "PVO_Escenarios.dbo.Esc_Velocidad";

                    copy.ColumnMappings.Add("ID", "id_articulo");
                    copy.ColumnMappings.Add("Año", "Año");
                    copy.ColumnMappings.Add("Valor", "Valor");

                    copy.ColumnMappings.Add("id_escenario", "id_escenario");
                    copy.ColumnMappings.Add("UserId", "UserId");
                    copy.ColumnMappings.Add("Valor_base", "Valor_base");
                    copy.ColumnMappings.Add("eliminado", "eliminado");

                    copy.WriteToServer(dataTable);
                }

                destinationConnection.Close();
            }

            db.Database.ExecuteSqlCommand(SQL_UPDATE_VELOCIDAD,
                new SqlParameter("@id_escenario", escEscenario.id_escenario));
            return true;
        }


        private void CreateXML(string path, DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' standalone='no'?>");
            sb.Append(Environment.NewLine);
            sb.Append("<!DOCTYPE NewDataSet SYSTEM '" + path + "\\StudentDTD1.dtd'>");
            sb.Append(Environment.NewLine);
            sb.Append(ds.GetXml());
            string ab = sb.ToString();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ab);
            doc.Save(path + "\\Students.xml");
        }


        private string GetConnectionString(string excelPath)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = excelPath;

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }

        private DataSet ReadExcelFile(string excelPath)
        {
            DataSet ds = new DataSet();

            string connectionString = GetConnectionString(excelPath);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                // DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                // foreach (DataRow dr in dtSheet.Rows[2])
                // {
                string sheetName = "Student$";

                //if (!sheetName.EndsWith("$"))
                //    continue;

                // Get all rows from the Sheet
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                DataTable dt = new DataTable();
                dt.TableName = sheetName.Replace("$", string.Empty);

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                ds.Tables.Add(dt);
                //}
                //string schema = ds.GetXmlSchema();
                cmd = null;
                conn.Close();
            }

            return ds;
        }

        private bool ValidateExcelSchema(string schema)
        {
            //XmlSchemaSet schemas;
            schemas = new XmlSchemaSet();
            schemas.Add("http://www.deitel.com/booklist", schema + "\\StudentsSchema.xsd");

            settings = new XmlReaderSettings();
            //settings.ProhibitDtd = false;
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.CheckCharacters = true;
            settings.ValidationType = ValidationType.DTD;
            settings.Schemas = schemas;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            Uri uri = new Uri(schema + "\\StudentDTD.dtd");
            //settings.XmlResolver.ResolveUri(uri, null);

            XmlReader reader = XmlReader.Create(schema + "\\Students.xml", settings);
            while (reader.Read()) ;
            if (valid)
            {
                writertbox("Document SCHEMA is valid");
            } // end if

            valid = true;
            reader.Close();

            return false;
        }

        private void ValidateExcelData(string Path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            //settings.ProhibitDtd = false;
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationType = ValidationType.DTD;

            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            XmlReader reader = XmlReader.Create(Path + "\\Students.xml", settings);


            // Parse the file.

            try
            {
                while (reader.Read()) ;

                if (valid)
                {
                    writertbox("Document contains valid data");
                } // end if
            }
            catch
            {
            }
        }

        private void ValidationCallBack(Object sender, ValidationEventArgs args)
        {
            //Display the validation error.  This is only called on error
            //bool m_Success = false; //Validation failed
            valid = false; // validation failed
            writertbox("Validation error: " + args.Message);
        }

        private void writertbox(string p)
        {
            //this.errorlist.Items.Add(p);
        }

        public string GetXML(DataTable dtDataTable, Boolean _blnSchema)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDataTable.Copy());
            if (_blnSchema)
                return ds.GetXmlSchema() + ds.GetXml();
            else
                return ds.GetXml();
        }

        //public string ValidateXML(string strSchemaFile, DataTable ds)
        //{

        //    strParseError = "";
        //    XmlReaderSettings settings = new XmlReaderSettings();

        //    settings.ValidationType = ValidationType.Schema;
        //    settings.Schemas.Add(....);
        //    settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs e)
        //    {
        //        Console.WriteLine("invalid: " + e.Message);
        //    };

        //    XmlReader reader = XmlReader.Create(new XmlTextReader(file), settings);


        //    var context = new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None);
        //    var xmlReader = new XmlTextReader(GetXML(ds, false), XmlNodeType.Element, context);


        //    var settings = new XmlReaderSettings();

        //    settings.ValidationType = ValidationType.Schema;
        //    settings.Schemas.Add("", strSchemaFile);
        //    settings.ValidationEventHandler += new ValidationEventHandler(ShowCompileErrors);

        //    XmlReader reader = XmlReader.Create(xmlReader, settings);

        //    return strParseError;
        //}

        public string ValidateXML(string strSchemaFile, DataTable ds)
        {
            strParseError = "";
            _error_repetido = false;

            XmlParserContext context =
                new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None);
            XmlTextReader xmlReader = new XmlTextReader(GetXML(ds, false), XmlNodeType.Element, context);

            var objValidator = new XmlValidatingReader(xmlReader);

            /* set the validation type to use an XSD schema */
            objValidator.ValidationType = ValidationType.Schema;
            //XmlSchemaCollection objSchemaCol = new XmlSchemaCollection();

            var objSchemaCol = new XmlSchemaCollection();

            objSchemaCol.Add("", strSchemaFile);
            objValidator.Schemas.Add(objSchemaCol);
            objValidator.ValidationEventHandler += new ValidationEventHandler(ShowCompileErrors);

            try
            {
                while (objValidator.Read())
                {
                    if (_error_repetido) break;
                }
            }
            catch (Exception objError)
            {
                throw new Exception(objError.Message);
            }
            finally
            {
                xmlReader.Close();
            }

            return strParseError;
        }

        private void ShowCompileErrors(object sender, ValidationEventArgs args)
        {
            if (UploadResult.FileUploadErrors == null)
            {
                UploadResult.FileUploadErrors = new List<FileUploadError>();
            }

            if (strParseError.Length == 0)
            {
                _primer_error = args.Message;
            }
            else
            {
                if (_primer_error == args.Message)
                {
                    _error_repetido = true;
                    return;
                }
            }

            strParseError += "::" + args.Message + "\r\n";
            FileUploadError fileUploadError = new FileUploadError()
            {
                Nombre_Hoja = _nombre_tabla,
                Message = args.Message.Replace("_x0020_", " ").Replace("_x002F_", @"/")
            };
            UploadResult.FileUploadErrors.Add(fileUploadError);
        }
    }
}
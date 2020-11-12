# TechApi
Prueba Backend Developer
#Tecnologia Utilizada#
Se utilizo tecnologia .Net para crear el WEB API 
Se utilizo una base de datos SQL Server 

#Instalacion#
El WebApi ya esta publicado en Azure y la URL es:https://servicioswebelaniin.azurewebsites.net/

Pero si se desea realizar la publicacion se deben seguir los siguientes pasos:

1.Se debe abrir el proyecto ServiciosWeb.sln en Visual Studio y generar el publish el proyecto se encuentra en la ruta \TechApi\ServiciosWeb
2.Desplegar el publish en un servidor Windows Server 2016 en adelante
3.Configurar el Pool del aplicativo en IIS
4.Configurar el SitioWeb en el IIS 
5.Copiar el publish en la carpeta creada desde el IIS
6.De igual forma se dejara el publish del proyecto en el repositorio
7.Modificar la cadena de conexion segun la base en donde se correra al script de migración de datos.


#Migration#
Correr el Script llamado MIGRATION_20201111_SQL.sql Ubicado en la carpeta TechApi\Migration 

#Coleccion Postman#
Url publica de la coleccion : https://www.postman.com/collections/e610a54288a73174295c
Ademas se agrego un archivo json en el repositorio por si no se puede visualizar la URL, el archivo se encuentra en la ruta origen del repositorio: TechCollection.postman_collection.json 
Documentacion de la coleccion de Postman: https://documenter.getpostman.com/view/13457850/TVemCVav
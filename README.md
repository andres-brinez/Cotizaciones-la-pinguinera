# Cotizaciones  la pingüinera

Este proyecto es una API que ayuda a la biblioteca la pingüinera para realizar los cálculoa de sus cotizaciones 

## Características

La API actualmente soporta las siguientes características:

1. Calcular el precio individual a pagar por un libro o novela.
2. Calcular el precio total a pagar por cualquier cantidad de libros y/o novelas, mostrando el valor total a pagar por todas las copias y los descuentos

## Tecnologías Utilizadas

- .NET Core
- FluentValidation
- C#

## Instalación

### Clonar el Repositorio

Para clonar este repositorio a tu máquina local, sigue estos pasos:

1. Abre una terminal.
2. Cambia el directorio de trabajo actual a la ubicación donde quieres clonar el repositorio.
3. Escribe `git clone`, y luego pega la URL del repositorio. Debería verse así: `git clone https://github.com/andres-brinez/Cotizaciones-la-pinguinera.git`.
4. Presiona Enter para crear tu clon local.

### Descargar el Proyecto

Si prefieres descargar el proyecto en lugar de clonarlo, puedes hacerlo así:

1. Navega al repositorio principal en GitHub.
2. Por encima de la lista de archivos, haz clic en el botón verde `Code`.
3. Haz clic en `Download ZIP`.
4. Extrae el archivo ZIP en tu máquina local.

### Instalar Dependencias y Ejecutar el Proyecto

Una vez que hayas clonado o descargado el proyecto, puedes instalar las dependencias y ejecutarlo así:

1. Navega a la carpeta del proyecto en tu terminal.
2. Instala las dependencias necesarias con `dotnet restore`.
3. Compila el proyecto con `dotnet build`.
4. Ejecuta la aplicación con `dotnet run`.

## Uso

La API expone dos endpoints:

- `POST /api/quotes/CalculateBookPay`: Calcula el precio total de un solo libro o novela.
- ![image](https://github.com/andres-brinez/Cotizaciones-la-pinguinera/assets/94869227/3fe0646a-b9b3-4dc8-a003-e496c1ffc50d)


- `POST /api/quotes/CalculateBooksPay`: Calcula el precio total de una lista de libros y/o novelas.
- ![image](https://github.com/andres-brinez/Cotizaciones-la-pinguinera/assets/94869227/ae6c2a63-cb82-47d0-b7e3-db4d0324820e)

  ## Archivo con las peticiones Postman
[Sofka Pinguinera.postman_collection.json](https://github.com/andres-brinez/Cotizaciones-la-pinguinera/files/14847010/Sofka.Pinguinera.postman_collection.json)


## Documentación Swagger
http://localhost:5275/swagger/index.html

## Licencia

[MIT](https://choosealicense.com/licenses/mit/)





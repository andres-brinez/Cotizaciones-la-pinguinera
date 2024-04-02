[Sofka Pinguinera.postman_collection.json](https://github.com/andres-brinez/Cotizaciones-la-pinguinera/files/14830422/Sofka.Pinguinera.postman_collection.json)
# Cotizaciones  la pingüinera

Este proyecto es una API que ayuda a la biblioteca la pingüinera para realizar los cálcusos de sus cotizaciones 

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
[Uploading{
	"info": {
		"_postman_id": "8a6de51a-ad10-4b7e-b24b-2c31fe5496fe",
		"name": "Sofka Pinguinera",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
		"_exporter_id": "23068830",
		"_collection_link": "https://lively-firefly-449085.postman.co/workspace/Backend-Ubunto~119561b0-a486-4f4d-8730-66868b42c085/collection/23068830-8a6de51a-ad10-4b7e-b24b-2c31fe5496fe?action=share&source=collection_link&creator=23068830"
	},
	"item": [
		{
			"name": "1 punto",
			"request": {
				"method": "POST",
				"header": [],
				"body": {
					"mode": "raw",
					"raw": "{\r\n    \"NameProvider\": \"Pablito\",\r\n    \"Title\": \"Las rosas\",\r\n    \"Seniority\":0,\r\n    \"OriginalPrice\": 100,\r\n    \"Type\": 1\r\n}",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "http://localhost:5275/api/quotes/CalculateBookPay",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5275",
					"path": [
						"api",
						"quotes",
						"CalculateBookPay"
					]
				}
			},
			"response": []
		},
		{
			"name": "punto 2",
			"request": {
				"method": "POST",
				"header": [],
				"body": {
					"mode": "raw",
					"raw": "[\r\n    {\r\n        \"NameProvider\": \"Pablita\",\r\n        \"Title\": \"Las rosasas\",\r\n        \"Seniority\": 0,  \r\n        \"OriginalPrice\": 100,  \r\n        \"Type\": 1\r\n    },\r\n    {\r\n        \"NameProvider\": \"Pablito\",\r\n        \"Title\": \"Las rosas\",\r\n        \"Seniority\": 2, \r\n        \"OriginalPrice\": 100,  \r\n        \"Type\":0\r\n    }\r\n    \r\n]\r\n",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "http://localhost:5275/api/quotes/CalculateBooksPay",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5275",
					"path": [
						"api",
						"quotes",
						"CalculateBooksPay"
					]
				}
			},
			"response": []
		}
	],
	"event": [
		{
			"listen": "prerequest",
			"script": {
				"type": "text/javascript",
				"exec": [
					""
				]
			}
		},
		{
			"listen": "test",
			"script": {
				"type": "text/javascript",
				"exec": [
					""
				]
			}
		}
	],
	"variable": [
		{
			"key": "URL_KEY\n",
			"value": " http://localhost:5275",
			"type": "string"
		}
	]
} Sofka Pinguinera.postman_collection.json…]()

  



## Licencia

[MIT](https://choosealicense.com/licenses/mit/)





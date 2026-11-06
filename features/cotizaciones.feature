# language: es
Característica: Solicitud de cotizaciones
  Como jefe de compras
  Quiero registrar cotizaciones de proveedores sobre solicitudes abiertas
  Para comparar ofertas y elegir la mejor

  Esquema del escenario: Validar el monto de la cotización
    Dado que existe una solicitud "SC-001" en estado "ABIERTA"
    Cuando el proveedor "900123456" registra una cotización por <monto> para la solicitud "SC-001"
    Entonces el sistema responde "<resultado>"

    Ejemplos:
      | monto   | resultado             |
      | 2500000 | Cotización registrada |
      | 0       | Monto inválido        |
      | -1000   | Monto inválido        |

  Escenario: Rechazar una cotización sobre una solicitud cerrada
    Dado que existe una solicitud "SC-002" en estado "CERRADA"
    Cuando el proveedor "900123456" registra una cotización por 2500000 para la solicitud "SC-002"
    Entonces el sistema responde "La solicitud no está abierta"

  Escenario: Rechazar una cotización duplicada del mismo proveedor
    Dado que existe una solicitud "SC-001" en estado "ABIERTA"
    Y el proveedor "900123456" ya cotizó la solicitud "SC-001"
    Cuando el proveedor "900123456" registra una cotización por 3000000 para la solicitud "SC-001"
    Entonces el sistema responde "El proveedor ya cotizó esta solicitud"

  Escenario: Aceptar cotizaciones de proveedores distintos para la misma solicitud
    Dado que existe una solicitud "SC-001" en estado "ABIERTA"
    Y el proveedor "900123456" ya cotizó la solicitud "SC-001"
    Cuando el proveedor "800987654" registra una cotización por 2700000 para la solicitud "SC-001"
    Entonces el sistema responde "Cotización registrada"
    Y hay 2 cotizaciones registradas

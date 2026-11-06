# language: es
Característica: Aprobación de órdenes de compra
  Como jefe de compras
  Quiero aprobar órdenes dentro del presupuesto del proveedor
  Para controlar el gasto de la empresa

  Escenario: Aprobar una orden dentro del presupuesto
    Dado que el proveedor "ACME" tiene un presupuesto disponible de $10.000.000
    Y existe una orden "OC-100" por $5.000.000 en estado "PENDIENTE"
    Cuando el jefe de compras aprueba la orden "OC-100"
    Entonces el sistema responde "Orden aprobada"
    Y la orden "OC-100" queda en estado "APROBADA"
    Y el presupuesto del proveedor "ACME" queda en $5.000.000

  Escenario: Rechazar una orden que excede el presupuesto
    Dado que el proveedor "ACME" tiene un presupuesto disponible de $10.000.000
    Y existe una orden "OC-101" por $15.000.000 en estado "PENDIENTE"
    Cuando el jefe de compras aprueba la orden "OC-101"
    Entonces el sistema muestra el mensaje "Presupuesto insuficiente"
    Y la orden "OC-101" queda en estado "PENDIENTE"
    Y el presupuesto del proveedor "ACME" queda en $10.000.000

  Escenario: No reaprobar una orden ya aprobada
    Dado que el proveedor "ACME" tiene un presupuesto disponible de $10.000.000
    Y existe una orden "OC-102" por $1.000.000 en estado "APROBADA"
    Cuando el jefe de compras aprueba la orden "OC-102"
    Entonces el sistema muestra el mensaje "La orden ya está aprobada"
    Y el presupuesto del proveedor "ACME" queda en $9.000.000

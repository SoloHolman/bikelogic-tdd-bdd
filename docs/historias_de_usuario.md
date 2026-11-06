# Historias de usuario

Backlog del incremento. Cada historia se desarrolla con TDD y se valida con un escenario BDD.

## Iteración 1: Proveedores

**HU-01. Registrar proveedor**
Como administrador, quiero registrar un proveedor con NIT, nombre y correo, para mantener actualizada la base de aliados comerciales.

Criterios de aceptación:
- El NIT es obligatorio, válido (solo dígitos, de 9 a 10 caracteres) y único.
- El nombre es obligatorio.
- El correo debe tener un formato válido.
- Un proveedor nuevo queda en estado "ACTIVO".

**HU-02. Desactivar proveedor**
Como administrador, quiero desactivar un proveedor que ya no usamos, para que no aparezca como activo sin perder su historial.

Criterios de aceptación:
- Un proveedor sin órdenes de compra activas pasa a estado "INACTIVO" y el sistema confirma "Proveedor desactivado".
- Un proveedor con órdenes activas no se puede desactivar.

## Iteración 2: Cotizaciones

**HU-03. Registrar cotización**
Como jefe de compras, quiero registrar la cotización de un proveedor sobre una solicitud, para comparar ofertas.

Criterios de aceptación:
- La solicitud debe existir y estar en estado "ABIERTA".
- El monto debe ser mayor que cero.

**HU-04. Evitar cotizaciones duplicadas**
Como jefe de compras, quiero que un proveedor no cotice dos veces la misma solicitud, para evitar ofertas contradictorias.

Criterios de aceptación:
- Un segundo intento del mismo proveedor sobre la misma solicitud se rechaza.
- Proveedores distintos sí pueden cotizar la misma solicitud.

## Iteración 3: Órdenes de compra

**HU-05. Aprobar orden de compra**
Como jefe de compras, quiero aprobar órdenes dentro del presupuesto del proveedor, para controlar el gasto.

Criterios de aceptación:
- Una orden cuyo monto no excede el presupuesto pasa a "APROBADA" y el presupuesto del proveedor se reduce en ese monto.
- Una orden que excede el presupuesto se rechaza y no modifica el presupuesto.

**HU-06. No reaprobar órdenes**
Como jefe de compras, quiero que una orden ya aprobada no se pueda aprobar de nuevo, para no descontar el presupuesto dos veces.

Criterios de aceptación:
- Aprobar una orden aprobada se rechaza y el presupuesto no cambia.

## Iteración 4: Inventario

**HU-07. Registrar producto**
Como encargado de inventario, quiero registrar productos, materia prima e insumos, para controlar las existencias.

Criterios de aceptación:
- El código es único.
- El stock inicial es mayor o igual a cero.
- La categoría es obligatoria.

**HU-08. Retirar stock**
Como encargado de inventario, quiero retirar unidades del inventario sin dejarlo negativo, para mantener existencias confiables.

Criterios de aceptación:
- Si el retiro es menor o igual al stock, se descuenta y el sistema responde "Retiro exitoso".
- Si el retiro supera el stock, el stock no cambia y el sistema responde "Stock insuficiente".

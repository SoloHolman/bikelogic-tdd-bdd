# language: es
Característica: Gestión de proveedores
  Como administrador del sistema
  Quiero registrar y desactivar proveedores
  Para mantener actualizada la base de aliados comerciales

  Escenario: Registrar un proveedor válido
    Dado que no existe un proveedor con NIT "900123456"
    Cuando registro el proveedor con NIT "900123456", nombre "ACME S.A.S." y correo "contacto@acme.com"
    Entonces el proveedor "900123456" queda en estado "ACTIVO"
    Y el sistema muestra el mensaje "Proveedor registrado exitosamente"

  Escenario: Rechazar un proveedor con NIT duplicado
    Dado que ya existe un proveedor con NIT "900123456"
    Cuando intento registrar otro proveedor con el mismo NIT
    Entonces el sistema muestra el mensaje "El NIT ya está registrado"
    Y solo existe 1 proveedor registrado

  Esquema del escenario: Validar el formato del correo
    Cuando registro un proveedor con correo "<correo>"
    Entonces el sistema responde "<resultado>"

    Ejemplos:
      | correo            | resultado                         |
      | contacto@acme.com | Proveedor registrado exitosamente |
      | sin-arroba.com    | Formato de correo inválido        |
      | usuario@          | Formato de correo inválido        |

  Escenario: Desactivar un proveedor sin órdenes activas
    Dado que existe un proveedor "ACME S.A.S." con NIT "900123456" sin órdenes de compra activas
    Cuando solicito desactivar el proveedor con NIT "900123456"
    Entonces el proveedor "900123456" queda en estado "INACTIVO"
    Y el sistema muestra el mensaje "Proveedor desactivado"

  Escenario: Rechazar desactivar un proveedor con órdenes activas
    Dado que existe un proveedor "ACME S.A.S." con NIT "900123456" con órdenes de compra activas
    Cuando solicito desactivar el proveedor con NIT "900123456"
    Entonces el sistema muestra el mensaje "No se puede eliminar un proveedor con órdenes activas"
    Y el proveedor "900123456" queda en estado "ACTIVO"

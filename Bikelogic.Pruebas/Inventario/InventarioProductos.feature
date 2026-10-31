# language: es
Característica: Inventario de productos, materia prima e insumos
  Como encargado de inventario
  Quiero registrar productos y controlar sus retiros
  Para mantener existencias confiables

  Esquema del escenario: Retiro de inventario
    Dado que el producto "<codigo>" tiene stock <stock>
    Cuando se retiran <retiro> unidades
    Entonces el stock final es <final>
    Y el sistema responde "<mensaje>"

    Ejemplos:
      | codigo   | stock | retiro | final | mensaje            |
      | PROD-001 | 10    | 3      | 7     | Retiro exitoso     |
      | PROD-002 | 5     | 5      | 0     | Retiro exitoso     |
      | PROD-003 | 2     | 5      | 2     | Stock insuficiente |

  Escenario: Registrar un producto con código único
    Cuando registro el producto "PROD-010" de la categoría "Repuestos" con stock inicial 10
    Entonces el producto "PROD-010" queda registrado con stock 10

  Escenario: Rechazar un producto con código duplicado
    Dado que ya existe el producto "PROD-010" con stock 10
    Cuando intento registrar otro producto con código "PROD-010"
    Entonces el sistema rechaza el registro
    Y el stock del producto "PROD-010" permanece en 10

  Escenario: Rechazar un producto con stock inicial negativo
    Cuando intento registrar el producto "PROD-011" de la categoría "Repuestos" con stock inicial -1
    Entonces el sistema rechaza el registro

  Escenario: Rechazar un producto sin categoría
    Cuando intento registrar el producto "PROD-012" de la categoría "" con stock inicial 5
    Entonces el sistema rechaza el registro

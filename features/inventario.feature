# language: es
Característica: Inventario de productos, materia prima e insumos
  Como encargado de inventario
  Quiero registrar productos y controlar sus retiros
  Para mantener existencias confiables

  Esquema del escenario: Retiro de inventario
    Dado que el producto "<codigo>" de la categoría "Repuestos" tiene stock <stock>
    Cuando se retiran <retiro> unidades del producto "<codigo>"
    Entonces el stock del producto "<codigo>" es <final>
    Y el sistema responde "<mensaje>"

    Ejemplos:
      | codigo   | stock | retiro | final | mensaje            |
      | PROD-001 | 10    | 3      | 7     | Retiro exitoso     |
      | PROD-002 | 5     | 5      | 0     | Retiro exitoso     |
      | PROD-003 | 2     | 5      | 2     | Stock insuficiente |

  Escenario: Registrar un producto con código único
    Cuando registro el producto "PROD-010" de la categoría "Repuestos" con stock inicial 10
    Entonces el sistema responde "Producto registrado exitosamente"
    Y el stock del producto "PROD-010" es 10

  Escenario: Rechazar un producto con código duplicado
    Dado que el producto "PROD-010" de la categoría "Repuestos" tiene stock 10
    Cuando registro el producto "PROD-010" de la categoría "Lubricantes" con stock inicial 5
    Entonces el sistema responde "El código del producto ya existe"
    Y el stock del producto "PROD-010" es 10

  Escenario: Rechazar un producto con stock inicial negativo
    Cuando registro el producto "PROD-011" de la categoría "Repuestos" con stock inicial -1
    Entonces el sistema responde "El stock inicial no puede ser negativo"

  Escenario: Rechazar un producto sin categoría
    Cuando registro el producto "PROD-012" sin categoría con stock inicial 5
    Entonces el sistema responde "La categoría es obligatoria"

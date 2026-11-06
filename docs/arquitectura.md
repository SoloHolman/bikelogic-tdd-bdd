# Arquitectura y decisiones de diseño

## Capas

Cada módulo de `src/bikelogic/` tiene la misma estructura, de manera que las reglas de negocio no dependen de cómo se guardan los datos:

| Archivo | Responsabilidad |
|---------|-----------------|
| `modelo.py` | Entidades y constantes del dominio (por ejemplo, los estados `ACTIVO` e `INACTIVO`). |
| `excepciones.py` | Errores de negocio del módulo, que heredan de `ErrorDeNegocio`. |
| `repositorio.py` | Almacenamiento. En esta versión es en memoria; puede sustituirse por una base de datos sin tocar el servicio. |
| `servicio.py` | Reglas de negocio. Es el único lugar donde se decide si una operación es válida. |

## Módulos

| Módulo | Reglas principales |
|--------|--------------------|
| `proveedores` | NIT válido y único, nombre obligatorio, correo con formato válido, desactivación solo sin órdenes activas. |
| `cotizaciones` | Solo sobre solicitudes abiertas, monto mayor que cero, un proveedor no cotiza dos veces la misma solicitud. |
| `ordenes_compra` | La orden no puede exceder el presupuesto del proveedor, no se reaprueba y la aprobación descuenta el presupuesto. |
| `inventario` | Código único, stock inicial mayor o igual a cero, categoría obligatoria y retiros que no dejan el stock negativo. |

## Decisiones

- **Errores de negocio.** Las reglas incumplidas lanzan excepciones que heredan de `ErrorDeNegocio`. Los escenarios BDD capturan esa clase base y comparan el mensaje.
- **Inyección de dependencias.** `ServicioProveedores` recibe una función `tiene_ordenes_activas`. Así el módulo de proveedores no depende del de órdenes de compra y ambos se integran en `tests/test_integracion.py`.
- **Montos enteros.** Los valores monetarios se manejan en pesos colombianos como enteros, porque no se usan centavos.
- **Una orden está activa** mientras esté pendiente de aprobación.
- **NIT válido:** solo dígitos, de 9 a 10 caracteres.

## Cómo se relacionan TDD y BDD

| Nivel | Herramienta | Qué verifica | Ubicación |
|-------|-------------|--------------|-----------|
| Unitario | pytest | Cada regla de negocio de un servicio, incluidos los casos límite. | `tests/` |
| Comportamiento | behave | El comportamiento descrito en lenguaje de negocio (Gherkin en español). | `features/` |
| Integración | pytest | La colaboración entre módulos. | `tests/test_integracion.py` |
| Calidad | flake8 y pytest-cov | Estilo del código y cobertura mínima del 80 %. | `.github/workflows/ci.yml` |

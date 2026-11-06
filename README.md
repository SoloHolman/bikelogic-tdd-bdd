# Bikelogic: TDD, BDD y Calidad Ágil con Python

[![CI](https://github.com/SoloHolman/bikelogic-tdd-bdd/actions/workflows/ci.yml/badge.svg)](https://github.com/SoloHolman/bikelogic-tdd-bdd/actions/workflows/ci.yml)

Incremento de **Bikelogic**, un ERP web para la gestión administrativa y operativa de un taller de motocicletas. Este repositorio implementa en Python la lógica de negocio de cuatro módulos (proveedores, cotizaciones, órdenes de compra e inventario) aplicando **TDD** (desarrollo guiado por pruebas), **BDD** (desarrollo guiado por comportamiento) y prácticas de **calidad ágil**.

Proyecto de la asignatura Ingeniería de Software II, Universidad de Cundinamarca, sede Chía.

## Estado del incremento

| Iteración | Módulo | Historias de usuario | Estado |
|-----------|--------|----------------------|--------|
| 1 | Proveedores | HU-01, HU-02 | Completada |
| 2 | Cotizaciones | HU-03, HU-04 | Completada |
| 3 | Órdenes de compra | HU-05, HU-06 | Completada |
| 4 | Inventario | HU-07, HU-08 | Completada |

Las historias de usuario y sus criterios de aceptación están en [`docs/historias_de_usuario.md`](docs/historias_de_usuario.md).

## Resultados de calidad

| Verificación | Resultado |
|--------------|-----------|
| Pruebas unitarias (pytest) | 44 pruebas superadas |
| Escenarios BDD (behave) | 23 escenarios en 4 funcionalidades, 83 pasos superados |
| Estilo del código (flake8) | Sin observaciones |
| Cobertura de código | Mínimo del 80 % exigido en la integración continua |

El diseño y las decisiones técnicas están en [`docs/arquitectura.md`](docs/arquitectura.md).

## Tecnologías

| Herramienta | Uso |
|-------------|-----|
| Python 3.14 | Lenguaje |
| pytest | Pruebas unitarias (TDD) |
| behave | Escenarios Gherkin en español (BDD) |
| pytest-cov | Cobertura de código |
| flake8 | Análisis estático de estilo |
| GitHub Actions | Integración continua |

## Estructura del repositorio

```
src/bikelogic/     Código de la aplicación (un paquete por módulo)
tests/             Pruebas unitarias con pytest
features/          Escenarios Gherkin (.feature)
features/steps/    Step definitions de behave
docs/              Documentación y backlog
.github/workflows/ Integración continua
```

## Instalación y ejecución

Requisitos: Python 3.14 y Git.

```
git clone https://github.com/SoloHolman/bikelogic-tdd-bdd.git
cd bikelogic-tdd-bdd
py -m venv .venv
.venv\Scripts\activate
python -m pip install -r requirements.txt
```

| Acción | Comando |
|--------|---------|
| Pruebas unitarias | `pytest` |
| Pruebas con cobertura | `pytest --cov=bikelogic --cov-report=term-missing` |
| Pruebas BDD | `behave` |
| Revisión de estilo | `flake8 src tests features` |

## Metodología de trabajo

- **Ramas:** `main` contiene siempre código que pasa todas las pruebas. Cada historia se desarrolla en una rama `feature/<módulo>` y se integra mediante *Pull Request*.
- **TDD:** cada funcionalidad sigue el ciclo rojo (prueba que falla), verde (código mínimo que la hace pasar) y refactor (mejorar sin cambiar el comportamiento). Cada fase se registra en un commit.
- **BDD:** el comportamiento se describe primero en escenarios Gherkin y se automatiza con behave.
- **Commits:** siguen la convención `tipo: descripción` (`test`, `feat`, `refactor`, `docs`, `chore`).
- Más detalle en [`CONTRIBUTING.md`](CONTRIBUTING.md).

## Ramas del repositorio

| Rama | Contenido |
|------|-----------|
| `main` | Incremento en Python con TDD y BDD |
| `csharp-talleres` | Talleres previos en C# con xUnit y Reqnroll |

## Equipo

| Integrante | Rol |
|------------|-----|
| (completar) | (completar) |

## Referencias

(completar en la entrega, en formato APA 7)

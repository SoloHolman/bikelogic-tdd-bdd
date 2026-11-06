# Guía de contribución

## Flujo de trabajo

1. Elegir una historia de usuario del tablero y moverla a **En progreso**.
2. Actualizar `main` y crear la rama de trabajo:
   ```
   git switch main
   git pull
   git switch -c feature/<módulo>
   ```
3. Aplicar el ciclo TDD (ver abajo), con un commit por fase.
4. Subir la rama (`git push -u origin feature/<módulo>`) y abrir un **Pull Request** hacia `main`.
5. Esperar la marca verde de la integración continua y la revisión de otro integrante.
6. Integrar el Pull Request y mover la tarjeta a **Hecho**.

## Ciclo TDD

| Fase | Qué se hace | Mensaje de commit |
|------|-------------|-------------------|
| Rojo | Escribir la prueba y comprobar que falla | `test: <comportamiento>` |
| Verde | Escribir el código mínimo para que pase | `feat: <comportamiento>` |
| Refactor | Mejorar el código manteniendo las pruebas en verde | `refactor: <qué se mejora>` |

## Convención de commits

`test`, `feat`, `refactor`, `docs`, `chore`. Mensajes en español, en imperativo y de máximo 72 caracteres.

## Definición de hecho

Una historia está hecha cuando:

- Tiene su escenario Gherkin y todos sus pasos están automatizados.
- Tiene pruebas unitarias para sus reglas de negocio, incluidos los casos límite.
- `pytest`, `behave` y `flake8` pasan sin errores.
- La cobertura del módulo es de al menos 80 %.
- El Pull Request fue revisado por otro integrante y la integración continua está en verde.
- El README refleja el nuevo estado.

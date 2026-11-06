"""Pasos compartidos por todos los escenarios."""
from behave import then


@then('el sistema responde "{mensaje}"')
@then('el sistema muestra el mensaje "{mensaje}"')
def paso_el_sistema_responde(context, mensaje):
    assert context.mensaje == mensaje, (
        f"Se esperaba {mensaje!r} y se obtuvo {context.mensaje!r}"
    )

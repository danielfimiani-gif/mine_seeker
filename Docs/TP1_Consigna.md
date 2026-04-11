# Inteligencia Artificial con Unity — Trabajo Práctico 1

## Entrega

La entrega deberá consistir en un link a un repositorio de GitHub en donde se encuentre alojado un proyecto de Unity. Si bien puede ser desarrollado en cualquier versión del editor que se prefiera, la entrega debe poder ser abierta desde **Unity 6 (versión 6000.0.36f1)** y seguir funcionando debidamente. Asimismo, debe evidenciarse que el proyecto fue elaborado usando versionado (debe tener varios commits, a contraposición de uno solo con la consigna completada).

## Ejercicio

El proyecto debe consistir en una simulación que utilice los conceptos de **pathfinding** y **finite state machines**. Las implementaciones de los mencionados conceptos deberá ser de su autoría (no utilizar soluciones provistas por el motor ni librerías de terceros), utilizando como referencia los ejemplos vistos en clase.

La simulación consistirá en un mapa que presenta obstáculos, con varios agentes dispuestos sobre él. Los agentes serán mineros que deben viajar por el mapa hacia la ubicación de una mina de oro, para empezar a excavar y extraer el oro. Cuando los mineros llenan su capacidad máxima de oro (o la veta se queda sin el mineral), los mismos deben retornar a su base, y descargarlo allí (para luego repetir el proceso). Deben existir diversas vetas, haciendo que los mineros le den prioridad a una que no esté ya ocupada por otro minero.

## Criterios de evaluación

Se dispone de un sistema de tiers, donde cada uno dispone de requisitos a cumplir para ser alcanzado. Contar con uno de los requisitos de un tier otorgará un punto. Con la finalidad de pasar de un tier a otro, es necesario cumplir con todos los requisitos indicados en él. Es decir, no se considerarán requisitos de un determinado tier si los pertenecientes al anterior no están todos logrados.

> Tecnicatura Superior en Programación de Videojuegos con Motores

### Tier B: 4 (cuatro)

- Los agentes pueden desplazarse de un punto a otro del mapa usando un algoritmo de pathfinding.
- Los agentes cambian su estado actual en base a una máquina de estados personalizada.
- Los mineros buscan, excavan y devuelven el oro acorde a la consigna.
- Existen obstáculos en el mapa, los cuales los agentes evitan en pos de llegar a su destino.

### Tier A: 7 (siete)

- Los agentes pueden cambiar su estrategia de pathfinding en tiempo de ejecución de la aplicación, pudiendo elegir entre **depth first**, **breadth first**, **Dijkstra** y **A***.
- Los agentes cuentan con al menos los estados de *"idle"*, *"yendo a minar"*, *"minando"* y *"descargando oro"*, con sus respectivas transiciones.
- Existe una interfaz que muestra cuánto oro total se recolectó y cuánto oro lleva cada agente (se actualiza a medida que se va minando).

### Tier S: 10 (diez)

- Los mineros recolectan y dejan el oro de forma progresiva, recurriendo a **corutinas** (o similar) dentro de sus estados de recolección y descarga.
- Existen zonas de mayor costo de viaje (agua, arena, etc), las cuales los agentes evitan para llegar antes a destino; el mayor costo se evidencia con la reducción de velocidad de movimiento (en caso de tener que pasar inevitablemente por dicha zona).
- Existen enemigos que pueden atacar y matar a los mineros, teniendo estos su propia máquina de estados, y agregando los estados necesarios también a los mineros para representar esta interacción (estado de **huída** y de **muerte**).

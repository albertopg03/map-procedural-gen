# Documentación Técnica - Algoritmos de Generación Procedural de Mapas

## Introducción

La **generación procedural de mapas** es un enfoque utilizado en el desarrollo de videojuegos para crear mapas, niveles o entornos de manera **automática**. Estos mapas no son predefinidos por diseñadores, sino que son generados en tiempo real usando algoritmos, lo que permite una gran variedad de resultados y la creación de mundos dinámicos y naturales.

Los algoritmos de generación procedural de mapas se basan en técnicas matemáticas, y algunos de los más conocidos son:

1. **Perlin Noise**: Utiliza un enfoque basado en ruido para crear variaciones suaves y naturales en los mapas.
2. **Automatas Celulares**: Crean mapas a través de reglas locales aplicadas a cada celda, simulando crecimiento o erosión.
3. **Random Walk**: Simula un "caminar aleatorio", ideal para generar cuevas o caminos.
4. **Random Maps**: Genera mapas aleatorios, generalmente con algún porcentaje de celdas llenas o vacías.
5. **Directional Tunnels**: Crea túneles con un ancho variable y desplazamiento aleatorio.
6. **Caves**: Genera mapas de tipo cueva mediante Perlin Noise o caminatas aleatorias.
7. **Smoothing**: Se utiliza para suavizar los resultados generados por otros algoritmos, eliminando irregularidades y creando mapas más estables.

En este documento, se explican detalladamente cada uno de los algoritmos que hemos implementado para la generación de mapas.

---

## Algoritmos Desarrollados

A continuación se describe cada uno de los algoritmos desarrollados, explicando cómo funcionan, sus características y el tipo de mapas que generan.

### 1. **PerlinNoise**

**Descripción:**  
Este algoritmo utiliza **ruido Perlin**, que es una función matemática que genera valores de forma suave y continua. Se usa principalmente para crear variaciones naturales, como alturas de terreno o biomas.

**Cómo Funciona:**
- Genera valores de **ruido** en el eje X.
- Usa estos valores para determinar la **altura** del mapa.
- Rellena las celdas del mapa desde la altura hasta el suelo, creando un **terreno de tipo montaña o colina**.

**Uso Típico:**  
Este algoritmo es ideal para **terrenos naturales** como montañas, colinas o mapas con características suaves.

---

### 2. **PerlinNoiseSmoothing**

**Descripción:**  
Este algoritmo es una **variación del PerlinNoise** que agrega un proceso de suavizado entre las celdas generadas, lo que crea transiciones más suaves entre diferentes áreas de altura.

**Cómo Funciona:**
- Usa **PerlinNoise** para determinar los valores iniciales de las alturas en el eje X.
- Divide el mapa en **intervalos** y suaviza las transiciones entre celdas cercanas utilizando interpolación.
- Rellena las celdas de cada sección suavizada desde la altura generada hasta el suelo.

**Uso Típico:**  
Ideal para **terrenos más suaves y naturales**, eliminando las transiciones abruptas que a veces se encuentran con el ruido puro de Perlin.

---

### 3. **RandomWalk**

**Descripción:**  
Este algoritmo genera mapas a través de una **caminata aleatoria**, donde la posición se desplaza aleatoriamente hacia arriba o hacia abajo, simulando el proceso de escavar una cueva o crear un pasillo.

**Cómo Funciona:**
- Empieza en una altura aleatoria.
- En cada iteración, se **sube** o **baja** aleatoriamente una unidad.
- Se rellena el mapa con **suelo** (valor 1) desde la altura actual hasta el fondo del mapa.

**Uso Típico:**  
Perfecto para **mapas de cuevas** o **túneles** generados de forma aleatoria, con un diseño no estructurado.

---

### 4. **RandomWalkSmoothing**

**Descripción:**  
Este algoritmo mejora la técnica de **RandomWalk** al agregar una **sección suave** en cada caminata, donde la altura de la celda no cambia abruptamente.

**Cómo Funciona:**
- Similar al algoritmo anterior, pero con la adición de **secciones de caminata** que tienen un **ancho mínimo** antes de realizar un cambio en la altura.
- La caminata continúa de manera aleatoria, pero con secciones más largas donde el movimiento es más suave.

**Uso Típico:**  
Genera **cuevas** o **pasillos** aleatorios con menos "zigzagueo", produciendo caminos más consistentes.

---

### 5. **PerlinNoiseCave**

**Descripción:**  
Este algoritmo genera **mapas de cuevas** utilizando **Perlin Noise** en dos dimensiones (X y Y), creando formas naturales y orgánicas.

**Cómo Funciona:**
- Usa **PerlinNoise** para generar valores para cada celda en el mapa.
- Si la celda es parte de una **cueva**, se marca como vacío (`0`); si no, se marca como terreno (`1`).
- Los bordes del mapa se pueden forzar a ser siempre **muros** si `areEdgeWalls` está activado.

**Uso Típico:**  
Ideal para generar mapas de **cuevas** o **mazmorras** donde las paredes y el vacío tienen formas orgánicas y suaves.

---

### 6. **RandomWalkCave**

**Descripción:**  
Este algoritmo genera **cuevas** mediante un proceso de **caminata aleatoria**, pero con la opción de moverse en **direcciones diagonales** para crear un mapa más complejo.

**Cómo Funciona:**
- Empieza en una posición aleatoria dentro del mapa.
- Realiza movimientos aleatorios (pueden ser **diagonales** o no) y elimina celdas de terreno (`0`).
- Se repite hasta eliminar un porcentaje definido de celdas.

**Uso Típico:**  
Ideal para crear **cuevas** con un diseño más **irregular** y **caótico**.

---

### 7. **DirectionalTunnel**

**Descripción:**  
Genera **túneles** con un **ancho variable**, permitiendo un control sobre su forma y desplazamiento a lo largo del mapa.

**Cómo Funciona:**
- Comienza en el centro del mapa y crea un túnel moviéndose hacia **arriba** o **abajo**.
- El **ancho del túnel** varía aleatoriamente con cada paso.
- La posición del túnel también puede cambiar aleatoriamente en el eje X.

**Uso Típico:**  
Perfecto para crear **túneles** o **pasillos** de diferentes tamaños, con una forma más natural y fluida.

---

### 8. **RandomMap**

**Descripción:**  
Este algoritmo crea un mapa completamente **aleatorio**, con un porcentaje de celdas llenas y vacías, y permite configurar si los bordes del mapa deben ser siempre **muros**.

**Cómo Funciona:**
- Asigna aleatoriamente a cada celda un valor de **0 o 1** dependiendo de un porcentaje configurado.
- Si `areEdgeWalls` es verdadero, coloca **muros** en los bordes del mapa.

**Uso Típico:**  
Ideal para mapas **aleatorios** y de **tamaño dinámico**, como niveles generados aleatoriamente o mapas con una **distribución de terreno** impredecible.

---

### 9. **AutomataCelularMoore**

**Descripción:**  
Este algoritmo utiliza la técnica de **autómata celular de Moore** para crear mapas mediante la aplicación de reglas locales basadas en las celdas vecinas (incluyendo diagonales).

**Cómo Funciona:**
- Genera un mapa aleatorio inicial.
- Aplica reglas basadas en el número de vecinos (8 vecinos posibles en total) para determinar si una celda debe convertirse en **suelo** (`1`) o **vacío** (`0`).
- Se repite el proceso varias veces para refinar el mapa.

**Uso Típico:**  
Genera mapas con un **crecimiento orgánico**, ideal para **mazmorras** o **cuevas** con un diseño más natural.

---

### 10. **AutomataCelularVonNeumann**

**Descripción:**  
Similar al algoritmo anterior, pero utilizando la **vecindad de Von Neumann** (solo las 4 celdas ortogonales) para determinar las celdas vecinas.

**Cómo Funciona:**
- Genera un mapa aleatorio inicial.
- Aplica reglas de autómata celular basadas solo en los **vecinos ortogonales** (arriba, abajo, izquierda y derecha).
- Se repite el proceso para refinar el mapa.

**Uso Típico:**  
Genera mapas con **formas más rectas** y **estructuradas**, como **pasillos** o **campos de batalla**.




# 🧭 Generator Assistant - Generador Procedural de Mapas para Unity

## 📘 Manual de Uso

### 🛠 Cómo acceder a la herramienta

Para comenzar a usar la herramienta en tu proyecto de Unity:

1. Abre tu proyecto en Unity.
2. En la barra superior de Unity, ve a `Tools ➝ Generador Procedural`.

Esto abrirá una ventana con el asistente de generación de mapas, una interfaz intuitiva para personalizar la creación de tus mapas.

<p align="center">
  <img src="/screenshots/tool1.PNG" alt="Captura interfaz general" width="45%"/>
  <img src="/screenshots/previewMap.PNG" alt="Captura previsualización mapa" width="45%" style="margin-left:10px"/>
</p>


### ⚙️ Configuración de parámetros

En la ventana del asistente encontrarás dos tipos de parámetros:

1. **Parámetros generales**: Son comunes a todos los algoritmos.
2. **Parámetros específicos**: Dependiendo del algoritmo que selecciones, se mostrarán opciones adicionales que te permitirán personalizar aún más el mapa generado.

### 👁 Previsualización y generación del mapa

Una vez hayas configurado los parámetros:

- Haz clic en **"Preview Map"** para ver una previsualización de tu mapa.
- Si te gusta la previsualización, haz clic en **"Spawn this map"** para añadir el mapa a la escena.

Si prefieres que el mapa se genere de forma **totalmente aleatoria** (sin tener en cuenta los parámetros), selecciona la opción de generación aleatoria. Esto hará que cada vez que inicies la escena se genere un mapa diferente, ideal para juegos con mapas dinámicos.

### 🧱 Configuración de tiles

Para que el mapa generado tenga un aspecto visual:

- Es necesario seleccionar los **tiles** que usarás en tu mapa (tilemap y tilebase).
- Se recomienda crear **tiles inteligentes en Unity**, para mejorar la estética y el aspecto natural de los mapas generados.

> [!NOTE]
> Si olvidas definir los tiles, se generará un mapa, pero sin la representación visual adecuada.

Cuando generes el mapa, se añadirá un GameObject llamado **"Generator"** en tu escena, el cual tiene adjunto un script llamado `MapGenerator`. Este script contiene varios componentes clave:

1. `MapData`: Un ScriptableObject con todos los datos generados. No modifiques este objeto directamente, ya que puede afectar el funcionamiento del generador.
2. `TileMap` y `TileBase`: Configuran el aspecto visual de los tiles en el mapa.
3. `RandomSeed`: Si lo activas, el mapa se generará aleatoriamente en cada ejecución.
4. `Seed`: La semilla usada para generar el mapa. Si te gusta un mapa en particular, guarda esta semilla y podrás reutilizarla para generar el mismo mapa con los mismos parámetros.

### 📁 Requisitos del proyecto

Para que todo funcione correctamente, es necesario crear una carpeta en la ruta `Assets/GeneratedMaps`. Esta carpeta es donde se almacenarán los ScriptableObjects generados por la herramienta.

> [!TIP]
> Si no se crea la carpeta, los objetos no se generarán correctamente. Este es un aspecto a mejorar en futuras versiones del generador, para evitar que el desarrollador tenga que crearla manualmente.

---

## 🧠 Documentación Técnica - Algoritmos de Generación Procedural de Mapas

La generación procedural permite crear mapas automáticamente utilizando algoritmos matemáticos. A continuación se describen los algoritmos implementados en este generador.

Para cada algoritmo, puedes añadir la imagen correspondiente en la carpeta `./screenshots/{nombre_algoritmo}.png`.

---

### 1. **PerlinNoise**

**Descripción:**  
Este algoritmo utiliza **ruido Perlin**, que es una función matemática que genera valores de forma suave y continua. Se usa principalmente para crear variaciones naturales, como alturas de terreno o biomas.

**Cómo Funciona:**
- Genera valores de **ruido** en el eje X.
- Usa estos valores para determinar la **altura** del mapa.
- Rellena las celdas del mapa desde la altura hasta el suelo, creando un **terreno de tipo montaña o colina**.

**Uso Típico:**  
Este algoritmo es ideal para **terrenos naturales** como montañas, colinas o mapas con características suaves.

![PerlinNoise](/screenshots/PerlinNoiseExample.PNG)

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

![PerlinNoiseSmoothing](/screenshots/perlinNoiseSmoothingExample.PNG)

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

![RandomWalk](/screenshots/randomWalkExample.PNG)

---

### 4. **RandomWalkSmoothing**

**Descripción:**  
Este algoritmo mejora la técnica de **RandomWalk** al agregar una **sección suave** en cada caminata, donde la altura de la celda no cambia abruptamente.

**Cómo Funciona:**
- Similar al algoritmo anterior, pero con la adición de **secciones de caminata** que tienen un **ancho mínimo** antes de realizar un cambio en la altura.
- La caminata continúa de manera aleatoria, pero con secciones más largas donde el movimiento es más suave.

**Uso Típico:**  
Genera **cuevas** o **pasillos** aleatorios con menos "zigzagueo", produciendo caminos más consistentes.

![RandomWalkSmoothing](/screenshots/randomWalkSmoothingExample.PNG)

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

![PerlinNoiseCave](/screenshots/perlinNoiseCaveExample.PNG)

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

![RandomWalkCave](/screenshots/randomWalkCaveExample.PNG)

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

![DirectionalTunnel](/screenshots/directionalTunnelExample.PNG)

---

### 8. **RandomMap**

**Descripción:**  
Este algoritmo crea un mapa completamente **aleatorio**, con un porcentaje de celdas llenas y vacías, y permite configurar si los bordes del mapa deben ser siempre **muros**.

**Cómo Funciona:**
- Asigna aleatoriamente a cada celda un valor de **0 o 1** dependiendo de un porcentaje configurado.
- Si `areEdgeWalls` es verdadero, coloca **muros** en los bordes del mapa.

**Uso Típico:**  
Ideal para mapas **aleatorios** y de **tamaño dinámico**, como niveles generados aleatoriamente o mapas con una **distribución de terreno** impredecible.

![RandomMap](/screenshots/randomMapExample.PNG)

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

![AutomataCelularMoore](/screenshots/automataCelularMooreExample.PNG)

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

![AutomataCelularVonNeumann](/screenshots/automataCelularVonNeumann.PNG)

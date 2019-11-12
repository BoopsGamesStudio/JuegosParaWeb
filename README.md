__Estructura GDD :__ 
 
<p align="center">
  <img src="Arte/31B9A9B0-C7EA-449F-A336-57E720E593D2.png" width="200">
</p>

* Twitter: https://twitter.com/Boops_Games
* Instagram: https://www.instagram.com/boopsgamesstudio/
* Itch.io: https://itch.io/profile/boops-games-studio
* Youtube:https://www.youtube.com/channel/UCdlggk1-f6dqdhcsiB29jWA

___

(Intrucciones)


___

# 1.- Índice

+ __[2.- Introducción](#intro)__
	+ __[2.1 Concepto del juego](#game_concept)__
	+ __[2.2 Características principales](#main_features)__
	+ __[2.3 Género](#genre)__
	+ __[2.4 Propósito y público objetivo](#target)__
	+ __[2.5 Jugabilidad (idea básica)](#basic_gameplay)__
	+ __[2.6 Estilo visual](#visual_style)__
	+ __[2.7 Alcance](#reach)__
	
+ __[3.- Mecánicas de juego](#mechanics)__
	+ __[3.1 Jugabilidad (en profundidad)](#gameplay)__
	+ __[3.2 Flujo de juego](#game_flow)__
	+ __[3.3 Personajes](#characters)__
	+ __[3.4 Movimiento](#movement)__
	+ __[3.5 Cómo jugar](#controls)__
	
+ __[4.- Interfaz](#interface)__

+ __[5. Arte](#art)__

+ __[6. Sistema de monetización](#money)__

+ __[7. El futuro del proyecto](#future)__

+ __[8. Historial de versiones](#changelog)__

+ __[9. Boops Games Studio](#credits)__

# <a name="intro"></a>2.- Introducción

Este es el documento de diseño de **Nombre del Juego**, un videojuego de navegador desarrolado por _Boops Games Studio_.
	
+ ## <a name="game_concept"></a> 2.1 Concepto del juego	

Juego multijugador de robots que exploran un escenario en busca de mejoras,objetos y armas para luego luchar e intentar ser el último superviviente 

+ ## <a name="main_features"></a>2.2 Características principales		
	
**Fase de exploración**: Se investiga un mundo lleno de mejoras, objetos y armas. Hacerse con las más valiosas te ayudará a alcanzar la victoria.

**Gran variedad de armas y personajes**: 4 personajes a elegir y 12 tipos de armas para luchar (¡Si las encuentras!)

**Batalla**: Tras explorar, toca darse mamporros... Con todo lo recolectado, tira del escenario a tus rivales antes de que te tiren a ti y proclámate vencedor.
	
+ ## 2.3 <a name="genre"></a>Género	
	
Juego multijugador con toques de exploración y battle royale
	
+ ## <a name="target"></a>2.4 Propósito y público objetivo
	
El juego se puede jugar de forma casual tiene un elemento de aletoriedad en los objetos que puede hacer que cualquier persona gane. Pero también de forma competitiva, ya que saberse los mapas de exploración de memoria ayuda a conseguir los mejores objetos.
La idea es buscar un equilibrio entre los dos públicos.
Por lo general, se espera un público joven.

+ ## <a name="basic_gameplay"></a>2.5 Jugabilidad(idea básica)
	
**Exploración**: Se explora el escenario y se cogen mejoras, objetos y armas.

**Batalla**: Con lo recogido en la anterior fase, se lucha por ser el último robot en matenerse en una plataforma sin caerse, pudiendo tirar a tus rivales con tus armas.

	
+ ## <a name="visual_style"></a>2.6 Estilo visual	
	
Se ha optado por usar voxel art en isométrico, dándole un estilo particular y fácil de moldear.

+ ## <a name="reach"></a>2.7 Alcance
	
Con nuestro equipo, planeamos tener las mecánicas base del juego, 4 personajes, 1 escenario y 12 tipos de armas.

# <a name="mechanics"></a>3.- Mecánicas de juego

+ ## <a name="gameplay"></a>3.1 Jugabilidad (en profundidad)	
	
Es un juego multijugador para hasta 4 personas. 
Se explora un escenario isométrico en busca de mejoras y armas. Los jugadores podrán verse y chocarse entre ellos,pero no podrán atacar en el mapa de exploración. Los objetos que se cojan dejarán de estar disponibles para el resto de jugadores.
En el escenario, habrá distintas *burbujas*. Dependiendo de su tamaño, las burbujas tendrán objetos comunes (pequeñas), objetos raros (medianas) y burbujas legendarias (grandes).

**Stats**

Los stats permitirán aumentar las habilidades del robot del jugador.
Los stats vendrán en burbujas de tres tamaños: pequeñas (1 ud), medianas (2 uds) y grandes (4uds). Cuanto más grande sea su tamaño, más difícil será obtenerlas.

Los tipos de stats son:

+ **Ataque**: Permite romper defensas y tirar más lejos a los jugadores.

+ **Defensa**: Cuanto más tengas, más golpes podrás recibir sin moverte mucho

+ **Velocidad**: Cómo de rápido se moverá el personaje y cómo de rápido serán los ataques.

Un robot podrá recoger como máximo 10 unidades de cada tipo de stat.
A su vez, dependiendo de la clase de personaje elegido, el robot tendrá unos stats base que aumentan el máximo base(pudiendo llegar a ser más de 10).
Por último, a los stats del robot, se le suma los del arma que coja.

**Cadencia**

Se trata de un stat exclusivo de las armas, se refiere al tiempo que se tarda entre uso y uso. A mayor cadencia, menor es el tiempo entre ataques.

**Clases de armas**

+ **Cuerpo a cuerpo**: Eficaz contra escudos porque al golpearlos paralizan al portador. Suelen tener alto ataque.

+ **Escudos**: Eficaz contra armas a distancia porque repelen el daño al recibir disparos de frente. Suelen tener alta defensa.

+ **Armas a distancia**: Eficaz contra armas cuerpo a cuerpo por poder atacar a distancia. Suelen tener alta cadencia.

Dentro de cada clase, habrá distintos tipos de arma según su rareza en las burbujas.

**Cuerpo a cuerpo**

+ **Común: Daga** At:Muy baja Df:Muy baja Vl:alta Cd:alta

+ **Raro 1: Espada** At:Alta Df:Baja Vl:Media Cd:Media
+ **Raro 2: Hacha** At:Muy Alta Df:Alta Vl:Baja Cd:Muy Baja

+ **Legendario: Lanza**  At:Media Df:Alta Vl:Media Cd:Alta

**Escudos**

+ **Común: Rodela** At:Baja Df:Baja Vl:Media Cd:Media

+ **Raro 1: Escudo de lágrima** At:Alta Df:Muy Alta Vl: Muy Baja Cd:Baja
+ **Raro 2: Mediano** At:Media Df:Media Vl:Media Cd:Media

+ **Legendario: Escudo punzante**  At:Muy Alta Df:Muy alta Vl:Baja Cd:Baja

**Distancia**

+ **Común: Pistola plasma** At:Baja Df:Muy Baja Vl:Media Cd:Alta

+ **Raro 1: Metralleta plasma** At:Muy Baja Df:Baja Vl:Alta Cd: Muy Alta
+ **Raro 2: Escopeta plasma** At:Muy Alta Df:Media Vl:Media Cd:Muy Baja

+ **Legendario: Cañón plasma**  At:Muy Alta Df: Alta Vl:Baja Cd:Media

Durante la fase de batalla, se tendrá en cuenta todo lo recogido en la fase de exploración.

El objetivo será tirar a los demás rivales. 

El robot que quede en pie al finalizar, gana la partida.

Si se tarda mucho, El escenario se hará más pequeño.

+ ## <a name="game_flow"></a>3.2 Flujo de juego	
	
Al principio del juego, se tendrá que buscar una sala y entrar
Se deberá escoger el personaje que se desee. Para darle más variedad, dos jugadores no podrán escoger el mismo personaje.

Se jugará la fase de exploración, y ttas pasar el tiempo especificado en los ajustes de la sala, se mostrarán los resultados de la fase de exploración y se pasará a la batalla.

Tras la batalla, se ven los resultados finales y se acaba la partida, volviendo al menú de selección de personaje.


+ ## <a name="characters"></a>3.3 Personajes	

A parte de su aspecto, los distintos personajes tienen características base, pudiendo tener más del máximo de stats de un tipo	
	
**Robot estándar**: at: 2 df: 2 vl: 2

**Robot ataque**: at: 4 df:1 vl:1

**Robot defensa**: at:1 df: 4 vl:1

**Robot velocidad**: at:1 df:1 vl:4

+ ## <a name="movement"></a>3.4 Movimiento
	
Em isométrico, los robots tienen control que recuerda a un vehículo, pero pudiendo girar sobre su eje.

+ ## <a name="controls"></a>3.5 Cómo Jugar

Hay dos modos de jugar, uno más pensado para dispositivos móviles y otro más pensado para PC

***Controles:***

* **Jugando en dispositivo móvil**: 

* **Jugando en PC**: 


# <a name="interface"></a>4.- Interfaz

+ ## 4.a Diagrama de flujo (Idea inicial)	



+ ## 4.b Menú principal



+ ## 4.c Créditos	




+ ## 4.d Redes sociales



+ ## 4.e Tienda	



+ ## 4.f Ajustes	



+ ## 4.g Selección de nivel	



+ ## 4.h Rankings	



+ ## 4.i Información de los personajes	



+ ## 4.j Juego	


+ ## 4.k Pausa	




+ ## 4.l Fin del nivel




# <a name="art"></a>5.- Arte 



# <a name="money"></a>6.- Sistema de monetización




# <a name="future"></a>7.- El futuro del proyecto



# <a name="changelog"></a>8.- Historial de versiones

+ (07/11/2019) Estructura básica del documento.
+ (12/11/2019) Añadida información inical del juego

# <a name="credits"></a>9.- Boops Games Studio

+ Alejandro Hernández Pérez -
+ Mario Marquez Balduque - 
+ Diego Sagredo de Miguel - 
+ Gabriel Muñoz Borchers - 
+ Carlos Ventura Padina González - 

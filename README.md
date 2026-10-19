# PartyShooting

Jogo 3D de **barraca de tiro ao alvo** de parque de diversões, feito na **Unity 6**. O jogador fica parado, os alvos surgem e cruzam a barraca, e cada munição tem velocidade, dano e efeito próprios.

## Apresentação

Slides do projeto, para o professor:

**[Ver apresentação no Canva](https://canva.link/k17vflog7ahwmm1)**

## Sobre o jogo

- O jogador e a tenda ficam parados; só a mira se move.
- Alvos (balão, pato, estrela e lata) são sorteados pelo spawner e andam entre pontos de início e fim.
- A bala sai do cano da arma, mas segue o ponto mirado no centro da tela.
- Quatro munições, escolhidas pelas teclas numéricas, com poderes diferentes.
- Menu inicial, tutorial e tela final.

## Controles

| Ação | Controle |
|---|---|
| Mirar | Mouse |
| Atirar | Botão esquerdo do mouse |
| Trocar de munição | Teclas `1`, `2`, `3` e `4` |
| Liberar o cursor | `Esc` (o próximo clique trava o cursor de novo, sem atirar) |
| Mover o jogador | Desligado por padrão (opção `Allow Movement` no Inspector, só para testes) |

## Munições

| Tecla | Munição | Velocidade | Dano | Efeito |
|---|---|---|---|---|
| 1 | Normal | 20 | 1 | Dano imediato |
| 2 | Pesada | 8 | 4 | Mais lenta, com dano maior |
| 3 | Fogo | 30 | 0 | Queimadura de 0,25 por segundo durante 4 s; cada bala cria uma queimadura independente, e elas acumulam |
| 4 | Gelo | 20 | 0,5 | Congela o alvo por 1 segundo |

> Os valores são parâmetros iniciais de balanceamento e podem mudar nos testes.

## Tecnologias

- **Unity 6** (versão `6000.3.6f1`)
- **Universal Render Pipeline (URP)**
- **Input System** (pacote novo de entrada da Unity)
- **C#** com Visual Studio
- Texturas do [OpenGameArt](https://opengameart.org)

## Como executar

1. Instale o **Unity Hub** e a versão **6000.3.6f1** do Unity.
2. Clone ou baixe este repositório.
3. No Unity Hub, clique em **Add** e selecione a pasta do projeto.
4. Abra o projeto e, em `Assets/Scenes`, abra a cena do jogo (`SampleScene`; para o fluxo completo, comece pela cena do menu).
5. Clique em **Play**.

## Estrutura do projeto

```
Assets/
├── Art/
│   ├── Materials/   # materiais dos alvos e do cenário
│   └── Textures/    # imagens (PNG com fundo transparente) e texturas
├── Prefabs/         # alvos, jogador, bala, arma e piso
├── Scenes/          # menu, tutorial, jogo e tela final
├── Scripts/         # código do jogo
└── Settings/        # configurações do URP
```

### Scripts principais

| Script | Função |
|---|---|
| `Player.cs` | Mira com o mouse, seleção de munição e disparo |
| `PlayerBullet.cs` | Movimento, trajetória e impacto dos projéteis |
| `Target.cs` | Movimento, vida, dano, pontuação e morte dos alvos |
| `TargetSpawner.cs` | Sorteia os 4 tipos de alvo e define os trajetos entre os pontos Spawn e End |
| `EnemyStatusEffects.cs` | Queimadura acumulativa e congelamento |
| `TypeTextAnimation.cs` | Texto digitado do tutorial, que avança com clique ou toque |

## Equipe

| Integrante | Responsabilidades |
|---|---|
| [Nome do integrante 1] | Mira central, sistema de munições e efeitos de status |
| Karine Vicente | Estilização e menus: menu, tutorial, tela final e texturas |
| [Nome do integrante 3] | Spawner de alvos, personagem e prefabs |

## Uso de inteligência artificial

Ferramentas de IA foram usadas como apoio de planejamento, código e depuração:

- **Claude**: planejamento do projeto, spawner de alvos, jogador, bala e correção de erros.
- **Gemini**: mecânica de texto do tutorial (avançar com clique ou toque).
- **Assistente de IA do integrante 1**: mira central e sistema de munições.

Testes, ajustes na cena e decisões de design foram feitos pela equipe.

## Problemas encontrados e soluções

| Problema | Solução |
|---|---|
| Erro de Input: o Input System estava ativo e o código usava a classe `Input` antiga | Scripts reescritos com o Input System novo |
| Mesh Collider do alvo não aceita virar trigger | Troca por Box Collider no prefab |
| Nenhum alvo spawnava, porque os pontos estavam vazios | Pares Spawn e End ligados no Inspector |
| O jogo parava ao atirar na parede | Bala reescrita com Raycast, sem depender de tag |
| Erros do pacote AI Assistant da Unity no Console | Pacote removido pelo Package Manager |

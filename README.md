# Task Manager ⌨️

## Descrição 
Projetado para otimizar a colaboração e o gerenciamento de tarefas dentro de uma equipe de desenvolvimento, esse sistema é estruturado em torno de dois papéis principais: Tech Leaders e Desenvolvedores. Ele facilita a criação e edição eficiente de tarefas em um banco de dados SQLite, atribuição e acompanhamento, fornecendo funcionalidades específicas para cada papel a fim de garantir um fluxo de trabalho tranquilo, como: adição de novos desenvolvedores, controle de acesso, logs, estatísticas, etc.

https://github.com/armentanoc/TaskManager/assets/88147887/7db1123f-f92f-4609-ae28-cf97b67d562f

## Utilização 
1. Clone esse repositório na sua máquina local:

   ```bash
   git clone https://github.com/armentanoc/TaskManager.git

## Recursos

### 🧑‍💻 Capacidades do Tech Leader

1. **Alterar Senha:** Líderes Técnicos têm a capacidade de alterar sua própria senha de acesso.
2. **Adicionar Novos Devs via JSON:** Líderes Técnicos podem adicionar novos desenvolvedores à equipe por meio de um arquivo JSON.
3. **Minhas Tarefas:** Acesso à lista de tarefas atribuídas ao Líder Técnico.
4. **Tarefas Relacionadas às Minhas:** Visualização de tarefas relacionadas ao trabalho do Líder Técnico.
5. **Tarefas do Time:** Exibição das tarefas de toda a equipe sob a liderança do Tech Leader.
6. **Aprovar Tarefa:** Capacidade de aprovar tarefas para iniciar, especialmente aquelas não iniciadas pelo próprio Tech Leader.
7. **Cancelar Tarefa:** Líderes Técnicos podem cancelar tarefas conforme necessário.
8. **Criar Tarefa:** Iniciar o processo de criação de uma nova tarefa, sendo automaticamente atribuídos como seus Tech Leaders e podendo atribuir outros devs como responsáveis.
9. **Criar Relacionamento:** Adicionar correlações entre tarefas, especificando relações lógicas de `ParentChild` ou `Dependency`.
10. **Modificar Status de Tarefa:** Alterar o status de uma tarefa (`Backlog`, `Concluida`, etc.).
11. **Modificar Deadline de Tarefa:** Ajustar prazos para conclusão de tarefas.
12. **Estatísticas de Tarefas do Time:** Acesso a estatísticas abrangentes sobre o `Status` das tarefas da sua equipe.
13. **Atribuir Dev a Tarefa:** Capacidade de atribuir um desenvolvedor específico a uma tarefa existente.

### 👩‍💻 Capacidades do Desenvolvedor


1. **Alterar Senha:** Desenvolvedores têm a capacidade de alterar sua própria senha de acesso.
2. **Criar Tarefa:** Iniciar o processo de criação de uma nova tarefa, sendo automaticamente atribuídos como seus responsáveis.
3. **Minhas Tarefas:** Acesso à lista das suas próprias tarefas ao Desenvolvedor.
4. **Tarefas Relacionadas:** Visualização de tarefas relacionadas às suas tarefas (relação estabelecida pelo TechLeader).
5. **Modificar Status de Tarefa:** Alterar o status de uma tarefa, como `Backlog`, `EmProgresso`, etc, sendo que só pode ser atribuída como `Concluida` se estiver aprovada pelo Tech Leader.

### 🔄 Correlação de Tarefas

1. **Lógica de Correlação:** Tarefas podem ser correlacionadas se envolverem funções diferentes, mas pertencerem à mesma área, com vínculos `ParentChild` ou `Dependency`.
2. **Gestão de Correlação:** Líderes Técnicos gerenciam a correlação de tarefas definindo relacionamentos entre elas.

### 🚀 Gestão de Desenvolvedores

1. **Adição de Desenvolvedores:** Líderes Técnicos podem adicionar desenvolvedores por meio de um arquivo no formato JSON, atualizável pelos Líderes Técnicos. 

### 🔐 Autenticação

1. **Controle de Acesso:** O acesso ao sistema requer uma chave de acesso única para todos os usuários. A chave padrão é `1234`, mas cada usuário tem a possibilidade de alterá-la. No banco de dados, são esses dados são salvos encriptados e nunca ficam visíveis, nem mesmo no console no momento da digitação. 

### 📜 Logs do Sistema
1. O projeto `TaskManager.Logger` é responsável por registrar os logs do sistema. Os logs são salvos no arquivo `log.txt` na pasta de execução do projeto base.

## Dependências 🛠️

### SQLite 📊

#### Instalação no Windows 🖥️

1. **Baixar o SQLite:**
   - Visite a [página de download do SQLite](https://www.sqlite.org/download.html).
   - Baixe o binário pré-compilado para Windows (32 bits ou 64 bits, dependendo do seu sistema). Certifique-se de baixar o .zip que contém as ferramentas de linhas de comando, incluindo o binário `sqlite3.exe`. No dia 2 de Janeiro de 2024, o arquivo .zip mais novo era o `sqlite-tools-win-x64-3440200.zip`.

2. **Extrair o SQLite:**
   - Descompacte o arquivo ZIP baixado em um diretório de sua escolha.

3. **Adicionar ao PATH (Opcional, mas recomendado) 🌐:**
   - Abra um Prompt de Comando como Administrador.
   - Execute o seguinte comando, substituindo `C:\Caminho\Para\SQLite` pelo caminho completo onde você descompactou o SQLite:

     ```bash
     setx PATH "%PATH%;C:\Caminho\Para\SQLite"
     ```

   - *Observação: Certifique-se de substituir `C:\Caminho\Para\SQLite` pelo caminho específico onde você descompactou o SQLite.*

4. **Testar a Instalação:**
   - Abra um novo Prompt de Comando.
   - Execute `sqlite3` para abrir o shell do SQLite.
   - Você deve ver o prompt do SQLite (`sqlite>`).

Agora está pronto para começar a trabalhar com o SQLite nesse projeto!

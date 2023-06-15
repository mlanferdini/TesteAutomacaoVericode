Feature: Testar Correios

Scenario: Cenário 1 - Buscar CEP inexistente nos correios
	Given Eu estou no site dos Correios
	When Eu preencho o campo "relaxation" com "80700000"
	And Eu clico no botão "//input[@id='relaxation']//following-sibling::button"
	Then Eu verifico que a página contém "Dados não encontrado"

Scenario: Cenário 2 - Buscar CEP existente nos correios
	Given Eu estou no site dos Correios
	When Eu preencho o campo "relaxation" com "01013-001"
	And Eu clico no botão "//input[@id='relaxation']//following-sibling::button"
	Then Eu verifico que o endereço seja em "Rua Quinze de Novembro, São Paulo/SP"

Scenario: Cenário 3 - Buscar CEP existente nos correios
	Given Eu estou no site dos Correios
	When Eu preencho o campo "objetos" com "SS987654321BR"
	And Eu clico no botão "//input[@id='objetos']//following-sibling::button"
    Then Eu verifico a exibição da mensagem "Objeto não encontrado na base de dados dos Correios."

# net-core-api-investimentos

API desenvolvida com o intuito de unificar dados de investimentos de fontes diversas, calcular o valor que deve ser pago de IR sobre cada investimento e calcular o valor de resgate baseado no tempo de custódia. 

## Estrutura do Projeto

Essa API foi dividida em 4 projetos para melhor controle e separação de responsabilidades. Temos também uma camada responsável pelos testes.

* Investimentos.Domain

        Implementa as classes e modelos da aplicação


* Investimentos.Service

        Implementa toda regra de negócio e cálculos necessários.


* Investimentos.Infra

        Contém a implementação de clients para prover dados vindos de fontes externas e classes que são utilizadas de forma geral na aplicação (utilitários, helpers, serializers).

* Investimentos.Api

        Implementa os controladores e serviços do projeto. Responsável por receber as requisições e direcioná-las ao serviço responsável pelo processamento da solicitação.

* Investimentos.Tests

        Projeto que contém todos os testes unitários necessários para garantir a confiabilidade e consistência da aplicação.


## Publicação
 
A API está publicada na plataforma Heroku, utilizando container e está disponível atráves do link: [https://api-investimentos-dev.herokuapp.com](https://api-investimentos-dev.herokuapp.com).


## Endpoints

* /

        Retorna um "ok" para informar que a aplicação está no ar.


* /investimentos

        Retorna uma listagem com todos os investimentos consolidados.
        Uma vez que esse endpoint é chamado, os dados retornados ficam armazenados em cache para futuras consultas. Os dados em cache expiram à 00:00 do dia seguinte.
        Para isso, foi utilizado o componente CacheMemory (contido no pacote Microsoft.Extensions.Caching.Memory).


* /metrics-text

        Retorna estatísticas de uso de cada endpoint da API, bem como médias dos tempos de resposta.
        Para cálculo das métricas, foi utilizado o componente App Metrics (contido no pacote App.Metrics.AspNetCore.Mvc).

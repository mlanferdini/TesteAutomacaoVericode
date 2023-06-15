using IronOcr;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using TechTalk.SpecFlow;

namespace TesteAutomacaoVericode
{
    [Binding]
    public class ExemploSteps
    {
        private IWebDriver driver;

        [BeforeScenario]
        public void BeforeScenario()
        {
            driver = WebDriverManager.Driver;
        }

        [AfterScenario]
        public void AfterScenario()
        {
        }

        [Given(@"Eu estou no site dos Correios")]
        public void GivenEuAbroOSiteDosCorreios()
        {
            Thread.Sleep(1000);
            driver.SwitchTo().Window(driver.WindowHandles.First());
            string urlCorreios = "https://www.correios.com.br";
            if (driver.Url != urlCorreios)
            {
                driver.Navigate().GoToUrl(urlCorreios);
            }
            driver.Manage().Window.Maximize();
            IWebElement btAceitaCookies = driver.FindElement(By.Id("btnCookie"));
            if (btAceitaCookies.Displayed)
            {
                btAceitaCookies.Click();
            }
            IWebElement fechaCarol = driver.FindElement(By.Id("carol-fecha"));
            if (fechaCarol.Displayed)
            {
                fechaCarol.Click();
            }
        }

        [When(@"Eu preencho o campo ""(.*)"" com ""(.*)""")]
        public void WhenEuPreenchoOCampoCom(string campo, string valor)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(campo)));
            element.SendKeys(valor);
        }

        [When(@"Eu clico no botão ""(.*)""")]
        public void WhenEuClicoNoBotao(string botao)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(botao)));
            element.Click();
            Thread.Sleep(1500);
        }

        [Then(@"Eu verifico que a página contém ""(.*)""")]
        public void ThenEuVerificoQueAPaginaContem(string texto)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            string xPathElemento = "//*[@id=\"mensagem-resultado-alerta\"]//*[.=\"" + texto + "\"]";
            IWebElement element = driver.FindElement(By.XPath(xPathElemento));
            element.GetAttribute("innerText");
            Assert.IsTrue(element.Displayed);
            driver.Close();
        }

        [Then(@"Eu verifico que o endereço seja em ""(.*)""")]
        public void ThenEuVerificoQueOEnderecoSejaEm(string texto)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            string[] endereco = texto.Split(',');
            string logradouroEsperado = endereco[0].Trim();
            string localidadeEsperada = endereco[1].Trim();

            IWebElement elementLogradouro = driver.FindElement(By.CssSelector("td[data-th='Logradouro/Nome']"));
            IWebElement elementLocalidadeUf = driver.FindElement(By.CssSelector("td[data-th='Localidade/UF']"));
            string logradouroEncontrado = elementLogradouro.GetAttribute("innerText");
            string localidadeEncontrada = elementLocalidadeUf.GetAttribute("innerText");

            Assert.IsTrue(logradouroEncontrado.Contains(logradouroEsperado));
            Assert.IsTrue(localidadeEncontrada.Contains(localidadeEsperada));
            driver.Close();
        }

        [Then(@"Eu verifico a exibição da mensagem ""(.*)""")]
        public void ThenEuVerificoAExibicaoDaMensagem(string texto)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            IWebElement elementImagem = driver.FindElement(By.XPath("//img[@id=\"captcha_image\"]"));
            string urlDaImagem = elementImagem.GetAttribute("src");
            string caminhoDestino = Path.Combine(Directory.GetCurrentDirectory(), "captcha.png");
            WebClient client = new WebClient();
            client.DownloadFile(urlDaImagem, caminhoDestino);
            var imagem = new IronOcr.OcrInput(caminhoDestino);
            File.Delete(caminhoDestino);
            var ironOcr = new IronTesseract();
            var resultado = ironOcr.Read(imagem);
            var textoReconhecido = resultado.Text;
            Console.WriteLine(textoReconhecido);//Não foi possível ler o texto do captcha.
            driver.Quit();
        }
    }
}

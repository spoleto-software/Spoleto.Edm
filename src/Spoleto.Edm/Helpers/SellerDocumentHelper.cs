using System.Xml.Linq;
using Spoleto.Edm.Upd;
using Spoleto.Edm;

namespace Spoleto.Edm.Helpers
{
    /// <summary>
    /// Разбор файла информации продавца УПД / УПДи формата 970 (приказ ФНС № ЕД-7-26/970@).
    /// </summary>
    public static class SellerDocumentHelper
    {
        /// <summary>
        /// Сведения из файла информации продавца.
        /// </summary>
        public static SellerDocumentInfo ReadSellerDocumentInfo(SignedDocumentInfo document)
        {
            var xml = XDocument.Parse(document.Content);

            var fileElement = xml.Root;
            if (fileElement?.Name.LocalName != "Файл")
            {
                throw new InvalidDataException($"Файл {document.FileName}: корневой элемент «{fileElement?.Name.LocalName}», ожидался «Файл».");
            }

            var documentElement = GetElement(document, fileElement, "Документ");
            var invoiceElement = GetElement(document, documentElement, "СвСчФакт");

            return new SellerDocumentInfo
            {
                FileId = GetAttribute(document, fileElement, "ИдФайл"),
                DateCreation = GetAttribute(document, documentElement, "ДатаИнфПр"),
                TimeCreation = GetAttribute(document, documentElement, "ВремИнфПр"),
                DocumentName = documentElement.Attribute("НаимДокОпр")?.Value,
                Function = ParseFunction(document, GetAttribute(document, documentElement, "Функция")),
                DocumentNumber = invoiceElement.Attribute("НомерДок")?.Value,
                DocumentDate = GetAttribute(document, invoiceElement, "ДатаДок"),
                DocumentUid = documentElement.Attribute("УИД")?.Value,
                IsUPDi = invoiceElement.Element("ИспрДок")?.Attribute("НомИспр") != null,
                Signature = document.Signature
            };
        }

        private static XElement GetElement(SignedDocumentInfo document, XElement parent, string name)
            => parent.Element(name)
                ?? throw new InvalidDataException($"Файл {document.FileName}: в элементе «{parent.Name.LocalName}» нет элемента «{name}».");

        private static string GetAttribute(SignedDocumentInfo document, XElement element, string name)
            => element.Attribute(name)?.Value
                ?? throw new InvalidDataException($"Файл {document.FileName}: у элемента «{element.Name.LocalName}» нет атрибута «{name}».");

        private static DocumentFunction ParseFunction(SignedDocumentInfo document, string value) => value switch
        {
            "СЧФ" => DocumentFunction.Invoice,
            "СЧФДОП" => DocumentFunction.InvoiceAndPrimaryDocument,
            "ДОП" => DocumentFunction.PrimaryDocument,
            "СвРК" => DocumentFunction.CommissionSale,
            "СвЗК" => DocumentFunction.CommissionPurchase,
            _ => throw new InvalidDataException($"Файл {document.FileName}: неизвестная функция документа «{value}».")
        };
    }
}

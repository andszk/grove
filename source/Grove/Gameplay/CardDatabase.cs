﻿namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using Lucene.Net.Analysis.Standard;
  using Lucene.Net.Documents;
  using Lucene.Net.Index;
  using Lucene.Net.QueryParsers;
  using Lucene.Net.Search;
  using Lucene.Net.Store;
  using Lucene.Net.Util;

  public class CardDatabase
  {
    private readonly StandardAnalyzer _analyzer = new StandardAnalyzer(Version.LUCENE_30);
    private readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();
    private readonly RAMDirectory _directory = new RAMDirectory();

    public int Count { get { return _cards.Count; } }
    public Card this[string name] { get { return _cards[name.ToLowerInvariant()]; } }


    public void Initialize(IEnumerable<Card> cards)
    {
      foreach (var card in cards)
      {
        _cards.Add(card.Name.ToLowerInvariant(), card);
      }

      BuildSearchIndex(cards);
    }

    private void BuildSearchIndex(IEnumerable<Card> cards)
    {
      using (var writer = new IndexWriter(_directory, _analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
      {
        foreach (var card in cards)
        {
          var document = new Document();

          document.Add(new Field("id", card.Name.ToLowerInvariant(), Field.Store.YES, Field.Index.NO));
          document.Add(new Field("text", card.Text.GetTextOnly(), Field.Store.YES, Field.Index.ANALYZED));
          document.Add(new Field("name", card.Name, Field.Store.YES, Field.Index.ANALYZED));
          document.Add(new Field("type", card.Type, Field.Store.YES, Field.Index.ANALYZED));
          document.Add(new Field("flavor", card.FlavorText.GetTextOnly(), Field.Store.YES, Field.Index.ANALYZED));
          document.Add(new Field("power", card.Power.GetValueOrDefault().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
          document.Add(new Field("toughness", card.Toughness.GetValueOrDefault().ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
          document.Add(new Field("cost", card.ConvertedCost.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

          writer.AddDocument(document);
        }

        writer.Commit();
        writer.Optimize();
      }
    }

    public IEnumerable<Card> Query(string searchStr)
    {
      if (string.IsNullOrEmpty(searchStr))
      {
        foreach (var card in _cards)
        {
          yield return card.Value;
        }

        yield break;        
      }

      var query = CreateQuery(searchStr);
      var searcher = new IndexSearcher(_directory);

      var hits = searcher.Search(query, _cards.Count).ScoreDocs;

      foreach (var hit in hits)
      {
        var document = searcher.Doc(hit.Doc);
        var cardName = document.Get("id");

        yield return _cards[cardName];
      }
    }

    private Query CreateQuery(string searchStr)
    {
      var parser = new QueryParser(Version.LUCENE_30, "name", _analyzer);

      Query query;
      try
      {
        query = parser.Parse(searchStr);
      }
      catch (ParseException)
      {
        query = parser.Parse("invalidsearchstringreturnsnoresults");
      }
      return query;
    }

    public List<string> GetCardNames()
    {
      return _cards.Values.Select(x => x.Name).OrderBy(x => x).ToList();
    }
  }
}
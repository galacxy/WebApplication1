﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Search
{
    
    public class LinkItem
    {
        string href;
        string text;

        public LinkItem(String link, String title)
        {
            Href = link;
            Text = title;
        }

        public LinkItem()
        {
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return Href + "," + Text;
        }

        public static Boolean operator ==(LinkItem L1, LinkItem L2)
        {
            return L1.Text.Equals(L2.Text);
        }

        public static Boolean operator !=(LinkItem L1, LinkItem L2)
        {
            return !(L1.Text.Equals(L2.Text));
        }
    }

    class preparedData
    {
        Int32 index;

        public Int32 Index
        {
            get { return index; }
            set { index = value; }
        }

        String link;

        public String Link
        {
            get { return link; }
            set { link = value; }
        }
        Int64 hits;

        public Int64 Hits
        {
            get { return hits; }
            set { hits = value; }
        }

        public static Boolean operator ==(preparedData pd1, preparedData pd2)
        {
            return pd1.Link.Equals(pd2.Link);
        }

        public static Boolean operator !=(preparedData pd1, preparedData pd2)
        {
            return !(pd1.Link.Equals(pd2.Link));
        }
    }

    class Dataset
    {
        int index;
        String link;
        String title;
        Int64 directHit;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }


        public String Link
        {
            get { return link; }
            set { link = value; }
        }


        public String Title
        {
            get { return title; }
            set { title = value; }
        }


        public Int64 DirectHit
        {
            get { return directHit; }
            set { directHit = value; }
        }

        public void directHitInc(Int64 count)
        {
            DirectHit += count;
        }

    }

    public class titleHits
    {
        string link;

        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        Int64 hits;

        public Int64 Hits
        {
            get { return hits; }
            set { hits = value; }
        }

        public titleHits()
        {
        }

        public titleHits(titleHits tH)
        {
            this.Hits = tH.Hits;
            this.Link = tH.Link;
        }

        public static Boolean operator >(titleHits pd1, titleHits pd2)
        {
            return (pd1.Hits > pd2.Hits ? true : false);
        }

        public static Boolean operator <(titleHits pd1, titleHits pd2)
        {
            return (pd1.Hits < pd2.Hits ? true : false);
        }

        public static Boolean operator ==(titleHits pd1, titleHits pd2)
        {
            return pd1.Hits == pd2.Hits;
        }

        public static Boolean operator !=(titleHits pd1, titleHits pd2)
        {
            return !(pd1.Hits == pd2.Hits);
        }

    }

    public class LinkFinder
    {
        public List<LinkItem> Find(string fileSource)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all <a>.....</a> matches in file.
            MatchCollection aTag = Regex.Matches(fileSource, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);


            // 2.
            // Loop over each match.
            foreach (Match m in aTag)
            {
                String value = m.Groups[1].Value;
                String link;
                LinkItem item = new LinkItem("", "");

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);

                if (m2.Success)
                {
                    link = m2.Groups[1].Value;
                    if (link.StartsWith("#") == true || link.Contains("(") || link.Contains("%"))
                    {
                        continue;
                    }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(link, "[A-Z][a-z]*:"))
                    {
                        continue;
                    }
                    else if (link.StartsWith("/wiki") == true)
                    {
                        item.Href = "http://en.wikipedia.org" + link;

                        // 4.
                        //Extracting link title

                        Match m3 = Regex.Match(value, @"title=\""(.*?)\""", RegexOptions.Singleline);
                        if (m3.Success)
                        {
                            item.Text = m3.Groups[1].Value;
                        }
                        list.Add(item);
                    }
                }

            }

            return list;
        }
    }





    class IndexedResults
    {
        public void makefile()
        {
            System.IO.StreamReader fr1 = null;
            System.IO.StreamReader fr2 = null;
            System.IO.StreamWriter fw1 = null;
            String[] Countries = { "india", "japan" };
            Dictionary<String, String> RawData = new Dictionary<String, String>();
            Dictionary<String, preparedData> PreparedData = new Dictionary<String, preparedData>();

            Int32 tweaks = 0;
            Int32 index = 0;
            foreach (String Country in Countries)
            {

                char[] seperator = { ',' };
                String RawDataLine, PreparedDataLine;
                try
                {
                    fr1 = new System.IO.StreamReader(Country + ".csv");
                    fr2 = new System.IO.StreamReader(Country + "Results.csv");
                    while ((RawDataLine = fr1.ReadLine()) != null)
                    {
                        String[] explodedRawDataLine = RawDataLine.Split(seperator);
                        if (RawData.ContainsKey(explodedRawDataLine[0]) == false)
                        {
                            RawData.Add(explodedRawDataLine[0], explodedRawDataLine[1]);
                        }

                    }
                    while ((PreparedDataLine = fr2.ReadLine()) != null)
                    {
                        String[] explodedPreparedRawDataLine = PreparedDataLine.Split(seperator);
                        preparedData temp = new preparedData();
                        temp.Link = explodedPreparedRawDataLine[1];
                        temp.Hits = Convert.ToInt64(explodedPreparedRawDataLine[2]);
                        if (PreparedData.ContainsKey(temp.Link) == false)
                        {
                            temp.Index = index;
                            PreparedData.Add(temp.Link, temp);
                            index++;
                        }
                        else
                        {
                            PreparedData[temp.Link].Hits += temp.Hits;
                            temp.Hits = PreparedData[temp.Link].Hits;
                            tweaks++;
                        }
                        Console.WriteLine(temp.Index + " " + temp.Link + " " + temp.Hits);
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (fr1 != null)
                    {
                        fr1.Close();
                    }
                    if (fr2 != null)
                    {
                        fr2.Close();
                    }
                }
            }
            try
            {
                fw1 = new System.IO.StreamWriter("mergerResults.csv", true);
                List<Dataset> indexedDataSet = new List<Dataset>();
                foreach (KeyValuePair<String, preparedData> pData in PreparedData)
                {
                    Dataset temp = new Dataset();
                    temp.DirectHit = pData.Value.Hits;
                    temp.Index = pData.Value.Index;
                    temp.Link = pData.Value.Link;
                    temp.Title = RawData[pData.Value.Link];
                    indexedDataSet.Add(temp);
                }
                foreach (Dataset ds in indexedDataSet)
                {
                    fw1.WriteLine(ds.Index + "," + ds.DirectHit + "," + ds.Title + "," + ds.Link);
                }
            }
            catch (Exception ie)
            {
                Console.WriteLine(ie.Message);
            }
            finally
            {
                if (fw1 != null)
                {
                    fw1.Close();
                }
            }
            Console.WriteLine(tweaks);
        }
    }

    public class LoadResult
    {
        Dictionary<String, titleHits> LoadedResultsSet;

        public Dictionary<String, titleHits> LoadedResultsSet1
        {
            get { return LoadedResultsSet; }
            set { LoadedResultsSet = value; }
        }

        public LoadResult(String ResultFile)
        {
            LoadedResultsSet = new Dictionary<String, titleHits>();
            LoadedResultsSet = loadResultSet(ResultFile);
            List<KeyValuePair<string, titleHits>> ResultList = LoadedResultsSet.ToList();
            ResultList.Sort(
                delegate(KeyValuePair<string, titleHits> firstPair, KeyValuePair<string, titleHits> nextPair)
                {
                    return (nextPair.Value.Hits.CompareTo(firstPair.Value.Hits));
                }
            );
            LoadedResultsSet.Clear();

            LoadedResultsSet = ResultList.ToDictionary(key => key.Key, value => value.Value);
        }

        public Dictionary<String, titleHits> loadResultSet(String DataFile)
        {
            Dictionary<String, titleHits> ResultSet = new Dictionary<String, titleHits>();
            System.IO.StreamReader fr = null;
            try
            {
                fr = new System.IO.StreamReader(DataFile);
                String fileLine;
                while ((fileLine = fr.ReadLine()) != null)
                {
                    String[] explodedLine = fileLine.Split('+');
                    titleHits temp = new titleHits();
                    temp.Hits = Convert.ToInt64(explodedLine[0]);
                    temp.Link = explodedLine[2];

                    if (ResultSet.ContainsKey(explodedLine[1]) == false)
                    {
                        ResultSet.Add(explodedLine[1], temp);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (fr != null)
                {
                    fr.Close();
                }
            }
            return ResultSet;
        }
    }


    public class ExecuteSearch
    {
        LoadResult LR;
        String commonWords;
        public enum SearchQuality
        {
            GOOD,
            BETTER,
            BEST
        }

        public ExecuteSearch(String ResultFile)
        {
            LR = new LoadResult(ResultFile);
            commonWords = @"\b(for|where|is|a|are|and|how|why|which|of|what|the|was|were|in|[^\w]+)\b";
        }

        public String cleanQuery(String query)
        {
            //String Query = Regex.Replace(query, @"[^\w]", " ");
            query = Regex.Replace(query, commonWords, " ", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"[\s]+", " ");
            return query.Trim().ToLowerInvariant();
        }

        

        public void getSearchResults(String query, out KeyValuePair<String, String>[] Results)
        {
            Results = null;
            if (LR.LoadedResultsSet1.Count == 0)
            {
                return;
            }
            query = cleanQuery(query);
            List<String> queryParts = new List<string>(query.Split(' '));
            Dictionary<String, String> ResultSet = new Dictionary<string, string>();
            ResultSet = retrieveResults(queryParts);
            String[] leastImpKey = { "International Standard Book Number",
                                    "Digital object identifier"
                                };
            String leastImpValue;
            foreach (var boguskey in leastImpKey)
            {
                if (query.Equals(boguskey) == false && ResultSet.TryGetValue(boguskey, out leastImpValue) == true)
                {
                    ResultSet.Remove(boguskey);
                }
            }
            Results = new KeyValuePair<string, string>[ResultSet.Count];
            Results = ResultSet.ToArray();
            ResultSet.Clear();
        }

        public Dictionary<String, String> retrieveResults(List<String> queryParts)
        {
            Dictionary<String, String> ResultSet = new Dictionary<string, string>();
            Dictionary<String, titleHits> BestMatches = new Dictionary<String, titleHits>();
            LinkItem mostValuable;
            BestMatches = findBestHitPage(queryParts, out mostValuable);
            if (mostValuable.Href != null && mostValuable.Href != "")
            {
                ResultSet.Add(mostValuable.Text, mostValuable.Href);
            }
            else
            {
                foreach (var match in BestMatches)
                {
                    mostValuable.Text = match.Key;
                    mostValuable.Href = match.Value.Link;
                    break;
                }
            }
            foreach (var match in BestMatches)
            {
                ResultSet.Add(match.Key, match.Value.Link);
            }
            //if (BestMatches.Count > 0)
            //{gg
            Dictionary<string, string> relatedLinks = new Dictionary<string, string>();
            relatedLinks = findRelatedLinks2(mostValuable.Text);
            if (relatedLinks.Count > 0)
            {
                foreach (var pair in relatedLinks)
                {
                    try
                    {
                        ResultSet.Add(pair.Key, pair.Value);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            //}
            return ResultSet;
        }

        public Dictionary<String, titleHits> findBestHitPage(List<String> queryTerms, out LinkItem mostValuable)
        {
            Dictionary<String, titleHits> exactWordsMatch = new Dictionary<string, titleHits>();
            Dictionary<String, titleHits> DerivedWordmatch = new Dictionary<String, titleHits>();
            mostValuable = new LinkItem();
            Boolean impDone = false;
            foreach (var result in LR.LoadedResultsSet1) //.OrderByDescending(key => key.Value)
            {
                int success = 0;
                foreach (var term in queryTerms)
                {
                    if (Regex.IsMatch(result.Key, term, RegexOptions.IgnoreCase) == true)
                    {
                        success++;
                    }
                }
                if (success == queryTerms.Count)
                {
                    SearchQuality decision = CompareTitleSuccess(queryTerms, result.Key);

                    if (decision == SearchQuality.BETTER)
                    {
                        exactWordsMatch.Add(result.Key, new titleHits(result.Value));
                    }
                    else if ( decision == SearchQuality.GOOD)
                    {
                        DerivedWordmatch.Add(result.Key, new titleHits(result.Value));
                    }
                    else if(decision == SearchQuality.BEST)
                    {
                        if (impDone == false)
                        {
                            mostValuable.Text = result.Key;
                            mostValuable.Href = result.Value.Link;
                            impDone = true;
                        }
                    }
                }
            }
            foreach(var match in DerivedWordmatch)
            {
                exactWordsMatch.Add(match.Key, new titleHits(match.Value));
            }
            return exactWordsMatch;
        }

        public SearchQuality CompareTitleSuccess(List<String> queryTerms, String pageTitle)
        {
            SearchQuality deviationValue = SearchQuality.GOOD;
 
            pageTitle = Regex.Replace(pageTitle, commonWords, " ", RegexOptions.IgnoreCase);
            pageTitle = Regex.Replace(pageTitle, @"[\s]+", " ");
            pageTitle = pageTitle.Trim().ToLowerInvariant();

            int success = 0;
            int derivedsuccess = 0;
            int exactsuccess = 0;

            foreach (var word in queryTerms)
            {
                MatchCollection m1 = Regex.Matches(pageTitle, @"^"+word+@"$", RegexOptions.IgnoreCase);
                if (m1.Count > 0)
                {
                    exactsuccess++;
                }
                else
                {
                    MatchCollection m2 = Regex.Matches(pageTitle, @"\b(" + word + @")\b", RegexOptions.IgnoreCase);
                    if (m2.Count > 0)
                    {
                        success++;
                    }
                    else
                    {
                        MatchCollection m3 = Regex.Matches(pageTitle, word , RegexOptions.IgnoreCase);
                        if (m3.Count > 0)
                        {
                            derivedsuccess++;
                        }
                    }
                }
            }

            if (exactsuccess == pageTitle.Split(' ').Length)
            {
                deviationValue = SearchQuality.BEST;
            }
            else if (exactsuccess>0 || (success > 0 && success <= pageTitle.Split(' ').Length))
            {
                deviationValue = SearchQuality.BETTER;
            }
            else if (derivedsuccess > 0 && derivedsuccess <= pageTitle.Split(' ').Length)
            {
               deviationValue = SearchQuality.GOOD;
            }
            return deviationValue;
        }

        public Dictionary<String, String> findRelatedLinks2(String pageTitle)
        {
            System.IO.StreamReader fr = null;
            String pageSource = "";
            Dictionary<string, string> relatedLinks = new Dictionary<string, string>();
            try
            {
                String dataSource = @"D:\rohit.bansal\WebApplication1\WebApplication1\DataStore\" + pageTitle + ".html";
                fr = new System.IO.StreamReader(dataSource);
                pageSource = fr.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (fr != null)
                {
                    fr.Close();
                }

                LinkFinder linkfinder = new LinkFinder();
                List<LinkItem> linkitems = linkfinder.Find(pageSource);
                Dictionary<String, titleHits> linkCounts = new Dictionary<string, titleHits>();
                foreach (var linkitem in linkitems)
                {
                    if (linkCounts.ContainsKey(linkitem.Text) == false)
                    {
                        titleHits temp = new titleHits();
                        temp.Hits = 1;
                        temp.Link = linkitem.Href;
                        linkCounts.Add(linkitem.Text, temp);
                    }
                    else
                    {
                        linkCounts[linkitem.Text].Hits++;
                    }
                }

                var SortedLinkCounts = from linkcount in linkCounts
                                       orderby linkcount.Value.Hits descending
                                       select linkcount;

                foreach (var link in SortedLinkCounts)
                {
                    try
                    {
                        relatedLinks.Add(link.Key, link.Value.Link);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return relatedLinks;
        }

        public List<KeyValuePair<String, String>> findRelatedLinks(String pageTitle, List<String> queryParts)
        {
            List<KeyValuePair<String, String>> relatedLinks = new List<KeyValuePair<string, string>>();
            System.IO.StreamReader fr = null;
            try
            {
                fr = new System.IO.StreamReader("DataStore\\" + pageTitle + ".html");
                String pageSource = fr.ReadToEnd();
                LinkFinder linkfinder = new LinkFinder();
                List<LinkItem> linkitems = linkfinder.Find(pageSource);
                int count = 0;

                foreach (var result in LR.LoadedResultsSet1)
                {
                    foreach (var part in queryParts)
                    {
                        if (result.Key.Equals(part))
                        {
                            try
                            {
                                KeyValuePair<string, string> local = new KeyValuePair<string, string>(result.Key, result.Value.Link);
                                if (relatedLinks.Contains(local) == false)
                                {
                                    relatedLinks.Add(new KeyValuePair<string, string>(result.Key, result.Value.Link));
                                    count++;
                                }
                            }
                            catch
                            {
                                Console.Write("");
                            }
                        }
                    }
                    foreach (var linkitem in linkitems)
                    {
                        if (result.Key.Equals(linkitem.Text))
                        {
                            try
                            {
                                KeyValuePair<string, string> local = new KeyValuePair<string, string>(result.Key, result.Value.Link);
                                if (relatedLinks.Contains(local) == false)
                                {
                                    relatedLinks.Add(new KeyValuePair<string, string>(result.Key, result.Value.Link));
                                    count++;
                                }
                            }
                            catch
                            {
                                Console.Write("");
                            }
                        }
                    }

                    if (count >= 10)
                    {
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (fr != null)
                {
                    fr.Close();
                }
            }
            return relatedLinks;
        }



        //public Dictionary<String, String> AskForSearch(String searchTerm, out Int32 NumResults)
        //{

        //        Dictionary<String, String> res = new Dictionary<string, string>();
        //        //res = getSearchResults(textInfo.ToTitleCase(searchTerm));
        //        res = getSearchResults(searchTerm);
        //        NumResults = res.Count;
        //        //Console.WriteLine("{0} results found for {1}", res.Count, searchTerm);
        //        //if (res.Count == 0)
        //        //{
        //        //    Console.WriteLine("Please check your spellings");
        //        //}
        //        //foreach (var pair in res)
        //        //{
        //        //    Console.WriteLine("Title = " + pair.Key);
        //        //    Console.WriteLine("Link = " + pair.Value);
        //        //    Console.WriteLine();
        //        //}
        //        //Console.WriteLine();

        //        //Console.Write("Search More (y:Yes or n:No): ");
        //        //try
        //        //{
        //        //    flag = Char.Parse(Console.ReadLine().ToString().Trim());
        //        //}
        //        //catch
        //        //{
        //        //    Console.WriteLine("Please press only 'y' or 'n' ");
        //        //}
        //        return res;
        //}

    }

    //class Program
    //{

    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            ExecuteSearch ES = new ExecuteSearch("preparedResults.csv");
    //            ES.AskForSearch();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.Message);
    //        }
    //    }
    //}
}
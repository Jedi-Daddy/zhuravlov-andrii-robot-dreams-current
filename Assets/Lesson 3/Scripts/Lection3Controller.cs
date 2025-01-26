using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lection3Controller : MonoBehaviour
{
    [SerializeField]
    private string inputElement; 
    [SerializeField]
    private List<string> items = new List<string>(); 

    // Print all elements from list
    [ContextMenu("Print List")]
    public void PrintList()
    {
        if (items.Count == 0)
        {
            Debug.Log("List is empty.");
        }
        else
        {
            Debug.Log(string.Join("\n", items));
        }
    }

    // Add element to the list
    [ContextMenu("Add Element")]
    public void AddElement()
    {
        if (!string.IsNullOrEmpty(inputElement))
        {
            if (!items.Contains(inputElement))
            {
                items.Add(inputElement);
                Debug.Log($"Added: {inputElement}");
            }
            else
            {
                Debug.Log($"Element '{inputElement}' already exists in the list. Cannot add duplicate.");
            }
        }
        else
        {
            Debug.Log("Input element is null or empty. Cannot add.");
        }
    }

    // Remove element from list
    [ContextMenu("Remove Element")]
    public void RemoveElement()
    {
        if (items.Remove(inputElement))
        {
            Debug.Log($"Removed: {inputElement}");
        }
        else
        {
            Debug.Log($"Element '{inputElement}' not found in the list.");
        }
    }

    // Clear all elements from list
    [ContextMenu("Clear List")]
    public void ClearList()
    {
        items.Clear();
        Debug.Log("List cleared.");
    }

    // Sort ascending and print 
    [ContextMenu("Sort and Print List Ascending")]
    public void SortAndPrintListAscending()
    {
        items.Sort();
        Debug.Log("List sorted in ascending order:\n" + string.Join("\n", items));
    }

    // Sort descending and print
    [ContextMenu("Sort and Print List Descending")]
    public void SortAndPrintListDescending()
    {
        items.Sort(Comparison);
        //items.Reverse(); // old version
        Debug.Log("List sorted in descending order:\n" + string.Join("\n", items));
    }

    // New way to reverse sort
    private int Comparison(string x, string y)
    {
        return String.Compare(y, x, StringComparison.Ordinal);
    }

    // Print only numbers
    [ContextMenu("Print Only Numeric Values")]
    public void PrintNumericValues()
    {
        var numericValues = items.FindAll(item => int.TryParse(item, out _));
        if (numericValues.Count == 0)
        {
            Debug.Log("No numeric values in the list.");
        }
        else
        {
            Debug.Log("Numeric values in the list:\n" + string.Join("\n", numericValues));
        }
    }

    // Print only symbols
    [ContextMenu("Print Only Alphabetic Values")]
    public void PrintAlphabeticValues()
    {
        var alphabeticValues = items.FindAll(item => System.Text.RegularExpressions.Regex.IsMatch(item, "^[a-zA-Z]+$"));
        if (alphabeticValues.Count == 0)
        {
            Debug.Log("No alphabetic values in the list.");
        }
        else
        {
            Debug.Log("Alphabetic values in the list:\n" + string.Join("\n", alphabeticValues));
        }
    }

    // Shuffle list
    [ContextMenu("Shuffle List")]
    public void ShuffleList()
    {
        var random = new System.Random();
        for (int i = items.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = items[i];
            items[i] = items[j];
            items[j] = temp;

        }
        Debug.Log("List shuffled:\n" + string.Join("\n", items));
    }

    // Count all elements in list
    [ContextMenu("Count List Elements")]
    public void CountListElements()
    {
        Debug.Log($"Total elements in the list: {items.Count}");
    }

    // Remove random element from list
    [ContextMenu("Remove Random Element")]
    public void RemoveRandomElement()
    {
        if (items.Count > 0)
        {
            var random = new System.Random();
            int index = random.Next(items.Count);
            string removedItem = items[index];
            items.RemoveAt(index);
            Debug.Log($"Randomly removed: {removedItem}\nUpdated list:\n" + string.Join("\n", items));
        }
        else
        {
            Debug.Log("List is empty. Nothing to remove.");
        }
    }

    // Find element in list and show his index
    [ContextMenu("Find Element")]
    public void FindElement()
    {
        if (!string.IsNullOrEmpty(inputElement))
        {
            int index = items.IndexOf(inputElement);
            if (index >= 0)
            {
                Debug.Log($"Element '{inputElement}' found at index {index}.");
            }
            else
            {
                Debug.Log($"Element '{inputElement}' not found in the list.");
            }
        }
        else
        {
            Debug.Log("Input element is null or empty.");
        }
    }
}

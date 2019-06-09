/**
 * Sorts items using Selection Sort, and returns a new array containing the
 * sorted items.
 *
 * Note that items are sorted in place, so the given Array is modified.
 *
 * @template T
 * @param {!Array<T>} items Items to be sorted. Items must be comparable using <
 *     and >.
 * @return {!Array<T>} The sorted items.
 */
function selectionSort(items) {
  for (let i = 0; i < items.length; i++) {
    // Find the minimum element in the unsorted Array from position i to the
    // end.
    let minIndex = i;
    for (let j = i + 1; j < items.length; j++) {
      // Found new minimum.
      if (items[j] < items[minIndex]) minIndex = j;
    }

    if (minIndex !== i) {
      // Swap items at i and minIndex.
      const swap = items[i];
      items[i] = items[minIndex];
      items[minIndex] = swap;
    }
  }

  return items;
}

module.exports = selectionSort;

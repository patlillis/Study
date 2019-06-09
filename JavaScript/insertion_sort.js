/**
 * Sorts items using Insertion Sort, and returns a new array containing the
 * sorted items.
 *
 * Note that items are sorted in place, so the given Array is modified.
 *
 * @template T
 * @param {!Array<T>} items Items to be sorted. Items must be comparable using <
 *     and >.
 * @return {!Array<T>} The sorted items.
 */
function insertionSort(items) {
  for (let i = 1; i < items.length; i++) {
    // Select value to be inserted.
    const valueToInsert = items[i];
    const position = i;

    // Find position to insert the value.
    for (let j = i; j > 0 && items[j - 1] > items[j]; j--) {
      const swap = items[j];
      items[j] = items[j - 1];
      items[j - 1] = swap;
    }
  }

  return items;
}

module.exports = insertionSort;

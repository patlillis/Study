#include "linked_list.h"
#include <vector>
#include "catch.hpp"

using IntNode = LinkedList<int>::Node;

// Verifies that the LinkedList's contents match the provided vector of values.
template <typename T>
bool verify_list_contents(const LinkedList<T>& l,
                          const std::vector<T>& values) {
    if (l.size() != (int)values.size()) return false;

    typename LinkedList<T>::Node* node = l.head();
    auto it = values.cbegin();
    while (node != nullptr) {
        if (node->value != *it) return false;
        node = node->next;
        it++;
    }

    return true;
}

TEST_CASE("head()") {
    LinkedList<int> l;

    SECTION("is initially nullptr") { REQUIRE(l.head() == nullptr); }
}

TEST_CASE("tail()") {
    LinkedList<int> l;

    SECTION("is initially nullptr") { REQUIRE(l.tail() == nullptr); }
}

TEST_CASE("is_empty()") {
    LinkedList<int> l;
    int i;

    SECTION("is initially true") { REQUIRE(l.is_empty() == true); }
}

TEST_CASE("size()") {
    LinkedList<int> l;

    SECTION("is initially 0") { REQUIRE(l.size() == 0); }
}

TEST_CASE("contains()") {
    LinkedList<int> l;

    SECTION("false for empty list") {
        REQUIRE(l.contains(0) == false);
        REQUIRE(l.contains(-159) == false);
    }

    SECTION("single-item list") {
        l.append(150);

        REQUIRE(l.contains(150) == true);
        REQUIRE(l.contains(0) == false);
    }

    SECTION("doesn't find item not in list") {
        l.append(150);
        l.append(-15);
        l.append(7);

        REQUIRE(l.contains(3) == false);
    }

    SECTION("finds first item in list") {
        l.append(150);
        l.append(-15);
        l.append(7);

        REQUIRE(l.contains(150) == true);
    }

    SECTION("finds item in middle of list") {
        l.append(150);
        l.append(-15);
        l.append(7);

        REQUIRE(l.contains(-15) == true);
    }

    SECTION("finds last item of list") {
        l.append(150);
        l.append(-15);
        l.append(7);

        REQUIRE(l.contains(7) == true);
    }
}

TEST_CASE("append()") {
    LinkedList<int> l;

    SECTION("appends to empty list") {
        l.append(5);

        REQUIRE(l.size() == 1);
        REQUIRE(l.head()->value == 5);
    }

    SECTION("appends multiple items in order") {
        l.append(5);
        l.append(15);
        l.append(-160);

        REQUIRE(verify_list_contents(l, {5, 15, -160}));
    }

    SECTION("updates head") {
        REQUIRE(l.head() == nullptr);

        l.append(5);
        REQUIRE(l.head()->value == 5);

        l.append(15);
        REQUIRE(l.head()->value == 5);

        l.append(-160);
        REQUIRE(l.head()->value == 5);
    }

    SECTION("updates tail") {
        REQUIRE(l.tail() == nullptr);

        l.append(5);
        REQUIRE(l.tail()->value == 5);

        l.append(15);
        REQUIRE(l.tail()->value == 15);

        l.append(-160);
        REQUIRE(l.tail()->value == -160);
    }

    SECTION("updates size") {
        REQUIRE(l.size() == 0);

        l.append(5);
        REQUIRE(l.size() == 1);

        l.append(15);
        REQUIRE(l.size() == 2);

        l.append(-160);
        REQUIRE(l.size() == 3);
    }
}

TEST_CASE("insert()") {
    LinkedList<int> l;

    SECTION("inserts into empty list") {
        l.insert(15, 0);

        REQUIRE(l.size() == 1);
        REQUIRE(l.head()->value == 15);
    }

    SECTION("inserts into empty list with non-zero index") {
        l.insert(15, 500);

        REQUIRE(l.size() == 1);
        REQUIRE(l.head()->value == 15);
    }

    SECTION("inserts into beginning of list") {
        l.append(15);

        l.insert(-50, 0);
        l.insert(150, 0);
        l.insert(7, 0);

        REQUIRE(verify_list_contents(l, {7, 150, -50, 15}));
    }

    SECTION("inserts into end of list") {
        l.append(15);

        l.insert(-50, 1);
        l.insert(150, 150);
        l.insert(7, 3);

        REQUIRE(verify_list_contents(l, {15, -50, 150, 7}));
    }

    SECTION("inserts into middle of list") {
        l.append(15);
        l.append(25);
        l.append(35);

        l.insert(20, 1);
        l.insert(40, 2);
        l.insert(60, 3);

        REQUIRE(verify_list_contents(l, {15, 20, 40, 60, 25, 35}));
    }

    SECTION("updates head") {
        REQUIRE(l.head() == nullptr);

        l.insert(15, 0);
        REQUIRE(l.head()->value == 15);

        l.insert(-150, 0);
        REQUIRE(l.head()->value == -150);

        l.insert(7, 2);
        REQUIRE(l.head()->value == -150);
    }

    SECTION("updates tail") {
        REQUIRE(l.tail() == nullptr);

        l.insert(15, 0);
        REQUIRE(l.tail()->value == 15);

        l.insert(-150, 1);
        REQUIRE(l.tail()->value == -150);

        l.insert(7, 1);
        REQUIRE(l.tail()->value == -150);
    }

    SECTION("updates size") {
        REQUIRE(l.size() == 0);

        l.insert(15, 0);
        REQUIRE(l.size() == 1);

        l.insert(-150, 1);
        REQUIRE(l.size() == 2);

        l.insert(7, 150);
        REQUIRE(l.size() == 3);
    }

    SECTION("returns false for invalid index") {
        REQUIRE(l.insert(15, -150) == false);
        REQUIRE(l.insert(15, -7) == false);
        REQUIRE(l.insert(15, -1) == false);
    }

    SECTION("returns true for valid index") {
        REQUIRE(l.insert(15, 0) == true);
        REQUIRE(l.insert(15, 7) == true);
        REQUIRE(l.insert(15, 1) == true);
    }
}

TEST_CASE("remove()") {
    LinkedList<int> l;

    SECTION("returns false for empty list") {
        REQUIRE(l.remove(15) == false);
        REQUIRE(l.remove(-150) == false);
    }

    SECTION("removes single item in list") {
        l.append(15);

        REQUIRE(l.remove(15) == true);
        REQUIRE(l.contains(15) == false);
    }

    SECTION("removes item from front of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove(15) == true);
        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.remove(7) == true);
        REQUIRE(l.size() == 0);
    }

    SECTION("removes item from end of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove(7) == true);
        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.size() == 0);
    }

    SECTION("removes item from middle of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.size() == 2);
    }

    SECTION("returns false for absent item") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove(-15) == false);
        REQUIRE(l.remove(17) == false);
    }

    SECTION("only removes first instance of item") {
        l.append(1);
        l.append(-15);
        l.append(2);
        l.append(-15);
        l.append(3);
        l.append(-15);
        l.append(4);

        REQUIRE(l.remove(-15) == true);
        REQUIRE(verify_list_contents(l, {1, 2, -15, 3, -15, 4}));

        REQUIRE(l.remove(-15) == true);
        REQUIRE(verify_list_contents(l, {1, 2, 3, -15, 4}));

        REQUIRE(l.remove(-15) == true);
        REQUIRE(verify_list_contents(l, {1, 2, 3, 4}));
    }

    SECTION("updates head") {
        l.append(15);
        l.append(-150);
        l.append(15);
        l.append(7);

        REQUIRE(l.head()->value == 15);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.head()->value == -150);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.head()->value == -150);
        REQUIRE(l.remove(15) == false);
        REQUIRE(l.head()->value == -150);
        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.head()->value == 7);
        REQUIRE(l.remove(7) == true);
        REQUIRE(l.head() == nullptr);
    }

    SECTION("updates tail") {
        l.append(15);
        l.append(-150);
        l.append(15);
        l.append(7);

        REQUIRE(l.tail()->value == 7);
        REQUIRE(l.remove(7) == true);
        REQUIRE(l.tail()->value == 15);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.tail()->value == 15);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.tail()->value == -150);
        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.tail() == nullptr);
    }

    SECTION("updates size") {
        l.append(15);
        l.append(-150);
        l.append(15);
        l.append(7);

        REQUIRE(l.size() == 4);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.size() == 3);
        REQUIRE(l.remove(15) == true);
        REQUIRE(l.size() == 2);
        REQUIRE(l.remove(-150) == true);
        REQUIRE(l.size() == 1);
        REQUIRE(l.remove(7) == true);
        REQUIRE(l.size() == 0);
    }
}

TEST_CASE("remove_node()") {
    LinkedList<int> l;

    SECTION("returns false for empty list") {
        IntNode* n = new IntNode(5);
        REQUIRE(l.remove_node(n) == false);
        delete n;
    }

    SECTION("removes single item in list") {
        l.append(15);

        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(l.contains(15) == false);
    }

    SECTION("removes item from front of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(verify_list_contents(l, {-150, 7}));
        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(verify_list_contents(l, {7}));
        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(verify_list_contents(l, {}));
    }

    SECTION("removes item from end of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove_node(l.tail()) == true);
        REQUIRE(verify_list_contents(l, {15, -150}));
        REQUIRE(l.remove_node(l.tail()) == true);
        REQUIRE(verify_list_contents(l, {15}));
        REQUIRE(l.remove_node(l.tail()) == true);
        REQUIRE(verify_list_contents(l, {}));
    }

    SECTION("removes item from middle of list") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove_node(l.head()->next) == true);
        REQUIRE(verify_list_contents(l, {15, 7}));
    }

    SECTION("returns false for nullptr") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.remove_node(nullptr) == false);
    }

    SECTION("returns false for absent item") {
        l.append(15);
        l.append(-150);
        l.append(7);

        IntNode* n = new IntNode(5);
        REQUIRE(l.remove_node(n) == false);
        delete n;
    }

    SECTION("updates head") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.head()->value == 15);
        REQUIRE(l.remove_node(l.head()->next));
        REQUIRE(l.head()->value == 15);
        REQUIRE(l.remove_node(l.head()));
        REQUIRE(l.head()->value == 7);
        REQUIRE(l.remove_node(l.head()));
        REQUIRE(l.head() == nullptr);
    }

    SECTION("updates tail") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.tail()->value == 7);
        REQUIRE(l.remove_node(l.head()->next));
        REQUIRE(l.tail()->value == 7);
        REQUIRE(l.remove_node(l.tail()));
        REQUIRE(l.tail()->value == 15);
        REQUIRE(l.remove_node(l.tail()));
        REQUIRE(l.tail() == nullptr);
    }

    SECTION("updates size") {
        l.append(15);
        l.append(-150);
        l.append(7);

        REQUIRE(l.size() == 3);
        REQUIRE(l.remove_node(l.head()->next) == true);
        REQUIRE(l.size() == 2);
        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(l.size() == 1);
        REQUIRE(l.remove_node(l.head()) == true);
        REQUIRE(l.size() == 0);
    }
}
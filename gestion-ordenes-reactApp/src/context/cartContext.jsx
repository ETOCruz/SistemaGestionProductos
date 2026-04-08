import { createContext, useReducer } from 'react'
import { cartReducer, cartInitialState, CART_ACTION_TYPES } from '@reducers/cartReducer'

export const CartContext = createContext()

function useCartReducer() {
    const [state, dispatch] = useReducer(cartReducer, cartInitialState)

    const addToCart = product => dispatch({
        type: CART_ACTION_TYPES.ADD_TO_CART,
        payload: product
    })

    const removeFromCart = product => dispatch({
        type: CART_ACTION_TYPES.REMOVE_FROM_CART,
        payload: product
    })

    const updateQuantity = (product, quantity) => dispatch({
        type: CART_ACTION_TYPES.UPDATE_QUANTITY,
        payload: { ...product, quantity }
    })

    const clearCart = () => dispatch({
        type: CART_ACTION_TYPES.CLEAR_CART
    })

    return { state, addToCart, removeFromCart, updateQuantity, clearCart }
}

export function CartProvider({ children }) {
    const { state, addToCart, removeFromCart, updateQuantity, clearCart } = useCartReducer()

    return (
        <CartContext.Provider value={{
            cart: state,
            addToCart,
            removeFromCart,
            updateQuantity,
            clearCart
        }}>
            {children}
        </CartContext.Provider>
    )
}

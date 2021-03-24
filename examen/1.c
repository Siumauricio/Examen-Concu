#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <mpi.h>

void process(int my_rank, int comm_sz, int *array, int tam, int *sum, int *mult);
int main(void)
{
    int comm_sz;
    int my_rank;
    int tamano;
    int array;
    int sum;
    int mult;
    srand(time(NULL));
    MPI_Init(NULL, NULL);
    MPI_Comm_size(MPI_COMM_WORLD, &comm_sz);
    MPI_Comm_rank(MPI_COMM_WORLD, &my_rank);
    process(my_rank, comm_sz, &array, tamano, &sum, &mult);
    MPI_Finalize();

    return 0;
}
void process(int my_rank, int comm_sz, int *array, int tam, int *sum, int *mult)
{

    if (my_rank == 0)
    {
        scanf("%d", &tam);
        if (tam >= 0 && tam <= 50)
        {
            array = malloc(tam * sizeof(int));
            for (int i = 0; i < tam; i++)
            {
                array[i] = rand() % 50;
            }
            for (int i = 0; i < tam; i++)
            {
                printf("x[%d] = %d\n", i, array[i]);
            }
            printf("\n");

            MPI_Send(array, tam + 1, MPI_INT, 1, 0, MPI_COMM_WORLD);
            MPI_Send(array, tam + 1, MPI_INT, 2, 0, MPI_COMM_WORLD);
        }
    }
    else
    {
        if (my_rank == 1)
        {
            array = malloc(tam * sizeof(int));

            MPI_Recv(array, (tam + 1), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
            int suma = 0;
            for (int i = 0; i < tam; i++)
            {
                if (array[i])
                {
                    if (array[i] % 2 == 0)
                    {
                        suma += array[i];
                    }
                }
            }

            sum = &suma;
            MPI_Send(sum, 1, MPI_INT, 3, 0, MPI_COMM_WORLD);
        }

        else if (my_rank == 2)
        {
            array = malloc(tam * sizeof(int));
            MPI_Recv(array, (tam + 1), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
            int multiplicacion = 1;
            for (int i = 0; i < tam - 1; i++)
            {
                if (array[i])
                {
                    if (array[i] % 2 != 0)
                    {
                        multiplicacion = multiplicacion * array[i];
                    }
                }
            }
            mult = &multiplicacion;
            printf("multiplicacion : %d\n", multiplicacion);
            //MPI_Send(mult, 4, MPI_INT, 4, 0, MPI_COMM_WORLD);
        }
        else if (my_rank == 3)
        {
            MPI_Recv(sum, 1, MPI_INT, 1, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
            int p = *sum;
            printf("Resultado suma: %d\n", p);
        }
        else if (my_rank == 4)
        {
            MPI_Recv(&mult, 1, MPI_INT, 3, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
            int mult2 = *mult;
            printf("Resultado multiplicacion: %d\n", mult2);
        }
    }
}